using System;

/*
 * Copyright (c) 1999, 2015, Oracle and/or its affiliates. All rights reserved.
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
    //using ClassFile =com.sun.tools.javac.jvm.ClassFile;
    //using PoolConstant =com.sun.tools.javac.jvm.PoolConstant;
    //using Api = com.sun.tools.javac.util.DefinedBy.Api;

    /// <summary>
    /// An abstraction for internal compiler strings. They are stored in
    ///  Utf8 format. Names are stored in a Name.Table, and are unique within
    ///  that table.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public abstract class Name : java.lang.common.api.Name // : javax.lang.model.element.Name, PoolConstant
    {
        public readonly Table table;

        protected internal Name(Table table)
        {
            this.table = table;
        }

        ///// <summary>
        ///// {@inheritDoc}
        ///// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        ////ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.LANGUAGE_MODEL) public boolean contentEquals(CharSequence cs)
        //public virtual bool contentEquals(CharSequence cs)
        //{
        //    return ToString().Equals(cs.ToString());
        //}

        //public override int poolTag()
        //{
        //    return ClassFile.CONSTANT_Utf8;
        //}

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual int length()
        {
            return ToString().Length;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual char charAt(int index)
        {
            return ToString()[index];
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual string subSequence(int start, int end)
        {
            return ToString().Substring(start, end - start);
        }

        /// <summary>
        /// Return the concatenation of this name and name `n'.
        /// </summary>
        public virtual Name append(Name n)
        {
            int len = getByteLength();
            sbyte[] bs = new sbyte[len + n.getByteLength()];
            getBytes(bs, 0);
            n.getBytes(bs, len);
            return table.fromUtf(bs, 0, bs.Length);
        }

        /// <summary>
        /// Return the concatenation of this name, the given ASCII
        ///  character, and name `n'.
        /// </summary>
        public virtual Name append(char c, Name n)
        {
            int len = getByteLength();
            sbyte[] bs = new sbyte[len + 1 + n.getByteLength()];
            getBytes(bs, 0);
            bs[len] = (sbyte)c;
            n.getBytes(bs, len + 1);
            return table.fromUtf(bs, 0, bs.Length);
        }

        /// <summary>
        /// An arbitrary but consistent complete order among all Names.
        /// </summary>
        public virtual int compareTo(Name other)
        {
            return other.getIndex() - this.getIndex();
        }

        /// <summary>
        /// Return true if this is the empty name.
        /// </summary>
        public virtual bool isEmpty()
        {
            return getByteLength() == 0;
        }

        /// <summary>
        /// Returns last occurrence of byte b in this name, -1 if not found.
        /// </summary>
        public virtual int lastIndexOf(sbyte b)
        {
            sbyte[] bytes = getByteArray();
            int offset = getByteOffset();
            int i = getByteLength() - 1;
            while (i >= 0 && bytes[offset + i] != b)
            {
                i--;
            }
            return i;
        }

        /// <summary>
        /// Does this name start with prefix?
        /// </summary>
        public virtual bool startsWith(Name prefix)
        {
            sbyte[] thisBytes = this.getByteArray();
            int thisOffset = this.getByteOffset();
            int thisLength = this.getByteLength();
            sbyte[] prefixBytes = prefix.getByteArray();
            int prefixOffset = prefix.getByteOffset();
            int prefixLength = prefix.getByteLength();

            if (thisLength < prefixLength)
            {
                return false;
            }

            int i = 0;
            while (i < prefixLength && thisBytes[thisOffset + i] == prefixBytes[prefixOffset + i])
            {
                i++;
            }
            return i == prefixLength;
        }

        /// <summary>
        /// Returns the sub-name starting at position start, up to and
        ///  excluding position end.
        /// </summary>
        public virtual Name subName(int start, int end)
        {
            if (end < start)
            {
                end = start;
            }
            return table.fromUtf(getByteArray(), getByteOffset() + start, end - start);
        }

        /// <summary>
        /// Return the string representation of this name.
        /// </summary>
        public override string ToString()
        {
            return Convert.utf2string(getByteArray(), getByteOffset(), getByteLength());
        }

        /// <summary>
        /// Return the Utf8 representation of this name.
        /// </summary>
        public virtual sbyte[] toUtf()
        {
            sbyte[] bs = new sbyte[getByteLength()];
            getBytes(bs, 0);
            return bs;
        }

        /* Get a "reasonably small" value that uniquely identifies this name
         * within its name table.
         */
        public abstract int getIndex();

        /// <summary>
        /// Get the length (in bytes) of this name.
        /// </summary>
        public abstract int getByteLength();

        /// <summary>
        /// Returns i'th byte of this name.
        /// </summary>
        public abstract sbyte getByteAt(int i);

        /// <summary>
        /// Copy all bytes of this name to buffer cs, starting at start.
        /// </summary>
        public virtual void getBytes(sbyte[] cs, int start)
        {
            Array.Copy(getByteArray(), getByteOffset(), cs, start, getByteLength());
        }

        /// <summary>
        /// Get the underlying byte array for this name. The contents of the
        /// array must not be modified.
        /// </summary>
        public abstract sbyte[] getByteArray();

        /// <summary>
        /// Get the start offset of this name within its byte array.
        /// </summary>
        public abstract int getByteOffset();

        public interface NameMapper<X>
        {
            X map(sbyte[] bytes, int offset, int len);
        }

        public virtual X map<X>(NameMapper<X> mapper)
        {
            return mapper.map(getByteArray(), getByteOffset(), getByteLength());
        }

        /// <summary>
        /// An abstraction for the hash table used to create unique Name instances.
        /// </summary>
        public abstract class Table
        {
            /// <summary>
            /// Standard name table.
            /// </summary>
            public readonly Names names;

            internal Table(Names names)
            {
                this.names = names;
            }

            /// <summary>
            /// Get the name from the characters in cs[start..start+len-1].
            /// </summary>
            public abstract Name fromChars(char[] cs, int start, int len);

            /// <summary>
            /// Get the name for the characters in string s.
            /// </summary>
            public virtual Name fromString(string s)
            {
                char[] cs = s.ToCharArray();
                return fromChars(cs, 0, cs.Length);
            }

            /// <summary>
            /// Get the name for the bytes in array cs.
            ///  Assume that bytes are in utf8 format.
            /// </summary>
            public virtual Name fromUtf(sbyte[] cs)
            {
                return fromUtf(cs, 0, cs.Length);
            }

            /// <summary>
            /// get the name for the bytes in cs[start..start+len-1].
            ///  Assume that bytes are in utf8 format.
            /// </summary>
            public abstract Name fromUtf(sbyte[] cs, int start, int len);

            /// <summary>
            /// Release any resources used by this table.
            /// </summary>
            public abstract void dispose();

            /// <summary>
            /// The hashcode of a name.
            /// </summary>
            protected internal static int hashValue(sbyte[] bytes, int offset, int length)
            {
                int h = 0;
                int off = offset;

                for (int i = 0; i < length; i++)
                {
                    h = (h << 5) - h + bytes[off++];
                }
                return h;
            }

            /// <summary>
            /// Compare two subarrays
            /// </summary>
            protected internal static bool Equals(sbyte[] bytes1, int offset1, sbyte[] bytes2, int offset2, int length)
            {
                int i = 0;
                while (i < length && bytes1[offset1 + i] == bytes2[offset2 + i])
                {
                    i++;
                }
                return i == length;
            }
        }
    }

}