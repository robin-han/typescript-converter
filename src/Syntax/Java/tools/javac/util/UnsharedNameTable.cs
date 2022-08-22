using System;

/*
 * Copyright (c) 1999, 2013, Oracle and/or its affiliates. All rights reserved.
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
    /// <summary>
    /// Implementation of Name.Table that stores names in individual arrays
    /// using weak references. It is recommended for use when a single shared
    /// byte array is unsuitable.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class UnsharedNameTable : Name.Table
    {
        public static Name.Table create(Names names)
        {
            return new UnsharedNameTable(names);
        }

        internal class WeakReference<T> : System.WeakReference where T : class
        {
            public WeakReference(T target)
            : this(target, false)
            { }

            public WeakReference(T target, bool trackResurrection) : base(target, trackResurrection)
            {
                if (target == null) throw new ArgumentNullException("target");
            }

            public T get()
            {
                return (T)this.Target;
            }
        }

        internal class HashEntry : WeakReference<NameImpl>
        {
            internal HashEntry next;
            internal HashEntry(NameImpl referent) : base(referent)
            {
            }
        }

        /// <summary>
        /// The hash table for names.
        /// </summary>
        private HashEntry[] hashes = null;

        /// <summary>
        /// The mask to be used for hashing
        /// </summary>
        private int hashMask;

        /// <summary>
        /// Index counter for names in this table.
        /// </summary>
        public int index;

        /// <summary>
        /// Allocator </summary>
        ///  <param name="names"> The main name table </param>
        ///  <param name="hashSize"> the (constant) size to be used for the hash table
        ///                  needs to be a power of two. </param>
        public UnsharedNameTable(Names names, int hashSize) : base(names)
        {
            hashMask = hashSize - 1;
            hashes = new HashEntry[hashSize];
        }

        public UnsharedNameTable(Names names) : this(names, 0x8000)
        {
        }


        public override Name fromChars(char[] cs, int start, int len)
        {
            sbyte[] name = new sbyte[len * 3];
            int nbytes = Convert.chars2utf(cs, start, name, 0, len);
            return fromUtf(name, 0, nbytes);
        }

        public override Name fromUtf(sbyte[] cs, int start, int len)
        {
            int h = hashValue(cs, start, len) & hashMask;

            HashEntry element = hashes[h];

            NameImpl n = null;

            HashEntry previousNonNullTableEntry = null;
            HashEntry firstTableEntry = element;

            while (element != null)
            {
                if (element == null)
                {
                    break;
                }

                n = element.get();

                if (n == null)
                {
                    if (firstTableEntry == element)
                    {
                        hashes[h] = firstTableEntry = element.next;
                    }
                    else
                    {
                        Assert.checkNonNull(previousNonNullTableEntry, "previousNonNullTableEntry cannot be null here.");
                        previousNonNullTableEntry.next = element.next;
                    }
                }
                else
                {
                    if (n.getByteLength() == len && Equals(n.bytes, 0, cs, start, len))
                    {
                        return n;
                    }
                    previousNonNullTableEntry = element;
                }

                element = element.next;
            }

            sbyte[] bytes = new sbyte[len];
            Array.Copy(cs, start, bytes, 0, len);
            n = new NameImpl(this, bytes, index++);

            HashEntry newEntry = new HashEntry(n);

            if (previousNonNullTableEntry == null)
            { // We are not the first name with that hashCode.
                hashes[h] = newEntry;
            }
            else
            {
                Assert.checkNull(previousNonNullTableEntry.next, "previousNonNullTableEntry.next must be null.");
                previousNonNullTableEntry.next = newEntry;
            }

            return n;
        }

        public override void dispose()
        {
            hashes = null;
        }

        internal class NameImpl : Name
        {
            internal NameImpl(UnsharedNameTable table, sbyte[] bytes, int index) : base(table)
            {
                this.bytes = bytes;
                this.index = index;
            }

            internal readonly sbyte[] bytes;
            internal readonly int index;

            public override int getIndex()
            {
                return index;
            }

            public override int getByteLength()
            {
                return bytes.Length;
            }

            public override sbyte getByteAt(int i)
            {
                return bytes[i];
            }

            public override sbyte[] getByteArray()
            {
                return bytes;
            }

            public override int getByteOffset()
            {
                return 0;
            }

        }

    }

}