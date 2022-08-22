using System;
using System.Collections.Generic;

/*
 * Copyright (c) 1999, 2012, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

namespace com.sun.tools.javac.util
{
    // using Api =com.sun.tools.javac.util.DefinedBy.Api;
    using com.sun.source.tree;
    using com.sun.tools.javac.util;
    using static com.sun.tools.javac.tree.JCTree;

    /// <summary>
    /// Implementation of Name.Table that stores all names in a single shared
    /// byte array, expanding it as needed. This avoids the overhead incurred
    /// by using an array of bytes for each name.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class SharedNameTable : Name.Table
    {
        // maintain a freelist of recently used name tables for reuse.
        // private static List<SoftReference<SharedNameTable>> freelist = List.nil();
        private static List<SharedNameTable> freelist = new List<SharedNameTable>();

        public static SharedNameTable create(Names names)
        {
            lock (typeof(SharedNameTable))
            {
                //while (freelist.nonEmpty())
                //{
                //    SharedNameTable t = freelist.head.get();
                //    freelist = freelist.tail;
                //    if (t != null)
                //    {
                //        return t;
                //    }
                //}
                foreach (var t in freelist)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                return new SharedNameTable(names);
            }
        }

        private static void dispose(SharedNameTable t)
        {
            lock (typeof(SharedNameTable))
            {
                // freelist = freelist.prepend(new SoftReference<SharedNameTable>(t));
                freelist.Insert(0, t);
            }
        }

        /// <summary>
        /// The hash table for names.
        /// </summary>
        private NameImpl[] hashes;

        /// <summary>
        /// The shared byte array holding all encountered names.
        /// </summary>
        public sbyte[] bytes;

        /// <summary>
        /// The mask to be used for hashing
        /// </summary>
        private int hashMask;

        /// <summary>
        /// The number of filled bytes in `names'.
        /// </summary>
        private int nc = 0;

        /// <summary>
        /// Allocator </summary>
        ///  <param name="names"> The main name table </param>
        ///  <param name="hashSize"> the (constant) size to be used for the hash table
        ///                  needs to be a power of two. </param>
        ///  <param name="nameSize"> the initial size of the name table. </param>
        public SharedNameTable(Names names, int hashSize, int nameSize) : base(names)
        {
            hashMask = hashSize - 1;
            hashes = new NameImpl[hashSize];
            bytes = new sbyte[nameSize];

        }

        public SharedNameTable(Names names) : this(names, 0x8000, 0x20000)
        {
        }

        public override Name fromChars(char[] cs, int start, int len)
        {
            int nc = this.nc;
            sbyte[] bytes = this.bytes = ArrayUtils.ensureCapacity(this.bytes, nc + len * 3);
            int nbytes = Convert.chars2utf(cs, start, bytes, nc, len) - nc;
            int h = hashValue(bytes, nc, nbytes) & hashMask;
            NameImpl n = hashes[h];
            while (n != null && (n.getByteLength() != nbytes || !Equals(bytes, n.index, bytes, nc, nbytes)))
            {
                n = n.next;
            }
            if (n == null)
            {
                n = new NameImpl(this);
                n.index = nc;
                n.length = nbytes;
                n.next = hashes[h];
                hashes[h] = n;
                this.nc = nc + nbytes;
                if (nbytes == 0)
                {
                    this.nc++;
                }
            }
            return n;
        }

        public override Name fromUtf(sbyte[] cs, int start, int len)
        {
            int h = hashValue(cs, start, len) & hashMask;
            NameImpl n = hashes[h];
            sbyte[] names = this.bytes;
            while (n != null && (n.getByteLength() != len || !Equals(names, n.index, cs, start, len)))
            {
                n = n.next;
            }
            if (n == null)
            {
                int nc = this.nc;
                names = this.bytes = ArrayUtils.ensureCapacity(names, nc + len);
                Array.Copy(cs, start, names, nc, len);
                n = new NameImpl(this);
                n.index = nc;
                n.length = len;
                n.next = hashes[h];
                hashes[h] = n;
                this.nc = nc + len;
                if (len == 0)
                {
                    this.nc++;
                }
            }
            return n;
        }

        public override void dispose()
        {
            dispose(this);
        }

        internal class NameImpl : Name
        {
            /// <summary>
            /// The next name occupying the same hash bucket.
            /// </summary>
            internal NameImpl next;

            /// <summary>
            /// The index where the bytes of this name are stored in the global name
            ///  buffer `byte'.
            /// </summary>
            internal int index;

            /// <summary>
            /// The number of bytes in this name.
            /// </summary>
            internal new int length;

            internal NameImpl(SharedNameTable table) : base(table)
            {
            }

            public override int getIndex()
            {
                return index;
            }

            public override int getByteLength()
            {
                return length;
            }

            public override sbyte getByteAt(int i)
            {
                return getByteArray()[index + i];
            }

            public override sbyte[] getByteArray()
            {
                return ((SharedNameTable)table).bytes;
            }

            public override int getByteOffset()
            {
                return index;
            }

            /// <summary>
            /// Return the hash value of this name.
            /// </summary>
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun2.tools.javac.util.DefinedBy.Api.LANGUAGE_MODEL) public int hashCode()
            public override int GetHashCode()
            {
                return index;
            }

            /// <summary>
            /// Is this name equal to other?
            /// </summary>
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun2.tools.javac.util.DefinedBy.Api.LANGUAGE_MODEL) public boolean equals(Object other)
            public override bool Equals(object other)
            {
                if (other is Name)
                {
                    return table == ((Name)other).table && index == ((Name)other).getIndex();
                }
                else
                {
                    return false;
                }
            }

        }

    }

}