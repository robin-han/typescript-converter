using System.Collections.Generic;

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
    /// A class for constructing lists by appending elements. Modelled after
    ///  java.lang.StringBuffer.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class ListBuffer<A>  // : AbstractQueue<A>
    {
        //        public static ListBuffer<T> of<T>(T x)
        //        {
        //            ListBuffer<T> lb = new ListBuffer<T>();
        //            lb.add(x);
        //            return lb;
        //        }

        //        /// <summary>
        //        /// The list of elements of this buffer.
        //        /// </summary>
        //        private List<A> elems;

        //        /// <summary>
        //        /// A pointer pointing to the last element of 'elems' containing data,
        //        ///  or null if the list is empty.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
        //        private List<A> last_Renamed;

        //        /// <summary>
        //        /// The number of element in this buffer.
        //        /// </summary>
        //        private int count;

        //        /// <summary>
        //        /// Has a list been created from this buffer yet?
        //        /// </summary>
        //        private bool shared;

        //        /// <summary>
        //        /// Create a new initially empty list buffer.
        //        /// </summary>
        //        public ListBuffer()
        //        {
        //            clear();
        //        }

        //        public void clear()
        //        {
        //            this.elems = List.nil();
        //            this.last_Renamed = null;
        //            count = 0;
        //            shared = false;
        //        }

        //        /// <summary>
        //        /// Return the number of elements in this buffer.
        //        /// </summary>
        //        public virtual int length()
        //        {
        //            return count;
        //        }
        //        public virtual int size()
        //        {
        //            return count;
        //        }

        //        /// <summary>
        //        /// Is buffer empty?
        //        /// </summary>
        //        public virtual bool isEmpty()
        //        {
        //            return count == 0;
        //        }

        //        /// <summary>
        //        /// Is buffer not empty?
        //        /// </summary>
        //        public virtual bool nonEmpty()
        //        {
        //            return count != 0;
        //        }

        //        /// <summary>
        //        /// Copy list and sets last.
        //        /// </summary>
        //        private void copy()
        //        {
        //            if (elems.nonEmpty())
        //            {
        //                List<A> orig = elems;

        //                elems = last_Renamed = List.of(orig.head);

        //                while ((orig = orig.tail).nonEmpty())
        //                {
        //                    last_Renamed.tail = List.of(orig.head);
        //                    last_Renamed = last_Renamed.tail;
        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// Prepend an element to buffer.
        //        /// </summary>
        //        public virtual ListBuffer<A> prepend(A x)
        //        {
        //            elems = elems.prepend(x);
        //            if (last_Renamed == null)
        //            {
        //                last_Renamed = elems;
        //            }
        //            count++;
        //            return this;
        //        }

        //        /// <summary>
        //        /// Append an element to buffer.
        //        /// </summary>
        //        public virtual ListBuffer<A> append(A x)
        //        {
        //            Assert.checkNonNull(x);
        //            if (shared)
        //            {
        //                copy();
        //            }
        //            List<A> newLast = List.of(x);
        //            if (last_Renamed != null)
        //            {
        //                last_Renamed.tail = newLast;
        //                last_Renamed = newLast;
        //            }
        //            else
        //            {
        //                elems = last_Renamed = newLast;
        //            }
        //            count++;
        //            return this;
        //        }

        //        /// <summary>
        //        /// Append all elements in a list to buffer.
        //        /// </summary>
        //        public virtual ListBuffer<A> appendList(List<A> xs)
        //        {
        //            while (xs.nonEmpty())
        //            {
        //                append(xs.head);
        //                xs = xs.tail;
        //            }
        //            return this;
        //        }

        //        /// <summary>
        //        /// Append all elements in a list to buffer.
        //        /// </summary>
        //        public virtual ListBuffer<A> appendList(ListBuffer<A> xs)
        //        {
        //            return appendList(xs.toList());
        //        }

        //        /// <summary>
        //        /// Append all elements in an array to buffer.
        //        /// </summary>
        //        public virtual ListBuffer<A> appendArray(A[] xs)
        //        {
        //            foreach (A x in xs)
        //            {
        //                append(x);
        //            }
        //            return this;
        //        }

        //        /// <summary>
        //        /// Convert buffer to a list of all its elements.
        //        /// </summary>
        //        public virtual List<A> toList()
        //        {
        //            shared = true;
        //            return elems;
        //        }

        //        /// <summary>
        //        /// Does the list contain the specified element?
        //        /// </summary>
        //        public virtual bool contains(object x)
        //        {
        //            return elems.Contains(x);
        //        }

        //        /// <summary>
        //        /// Convert buffer to an array
        //        /// </summary>
        //        public virtual T[] toArray<T>(T[] vec)
        //        {
        //            return elems.toArray(vec);
        //        }
        //        public virtual object[] toArray()
        //        {
        //            return toArray(new object[size()]);
        //        }

        //        /// <summary>
        //        /// The first element in this buffer.
        //        /// </summary>
        //        public virtual A first()
        //        {
        //            return elems.head;
        //        }

        //        /// <summary>
        //        /// Return first element in this buffer and remove
        //        /// </summary>
        //        public virtual A next()
        //        {
        //            A x = elems.head;
        //            if (elems.Count > 0)
        //            {
        //                elems = elems.tail;
        //                if (elems.Count == 0)
        //                {
        //                    last_Renamed = null;
        //                }
        //                count--;
        //            }
        //            return x;
        //        }

        //        /// <summary>
        //        /// An enumeration of all elements in this buffer.
        //        /// </summary>
        //        public virtual IEnumerator<A> iterator()
        //        {
        //            return new IteratorAnonymousInnerClass(this);
        //        }

        //        private class IteratorAnonymousInnerClass : IEnumerator<A>
        //        {
        //            private readonly ListBuffer<A> outerInstance;

        //            public IteratorAnonymousInnerClass(ListBuffer<A> outerInstance)
        //            {
        //                this.outerInstance = outerInstance;
        //                outerInstance.elems = outerInstance.elems;
        //            }

        //            internal List<A> outerInstance.elems;
        //            public virtual bool hasNext()
        //            {
        //                return outerInstance.elems.Count > 0;
        //            }
        //            public virtual A next()
        //            {
        //                if (outerInstance.elems.Count == 0)
        //                {
        //                    throw new NoSuchElementException();
        //                }
        //                A elem = outerInstance.elems.head;
        //                outerInstance.elems = outerInstance.elems.tail;
        //                return elem;
        //            }
        //            public virtual void remove()
        //            {
        //                throw new System.NotSupportedException();
        //            }
        //        }

        //        public virtual bool add(A a)
        //        {
        //            append(a);
        //            return true;
        //        }

        //        public virtual bool remove(object o)
        //        {
        //            throw new System.NotSupportedException();
        //        }

        //        public virtual bool containsAll<T1>(ICollection<T1> c)
        //        {
        //            foreach (object x in c)
        //            {
        //                if (!contains(x))
        //                {
        //                    return false;
        //                }
        //            }
        //            return true;
        //        }

        //        public virtual bool addAll<T1>(ICollection<T1> c) where T1 : A
        //        {
        //            foreach (A a in c)
        //            {
        //                append(a);
        //            }
        //            return true;
        //        }

        //        public virtual bool removeAll<T1>(ICollection<T1> c)
        //        {
        //            throw new System.NotSupportedException();
        //        }

        //        public virtual bool retainAll<T1>(ICollection<T1> c)
        //        {
        //            throw new System.NotSupportedException();
        //        }

        //        public virtual bool offer(A a)
        //        {
        //            append(a);
        //            return true;
        //        }

        //        public virtual A poll()
        //        {
        //            return next();
        //        }

        //        public virtual A peek()
        //        {
        //            return first();
        //        }

        //        public virtual A last()
        //        {
        //            return last_Renamed != null ? last_Renamed.head : default(A);
        //        }
    }

}