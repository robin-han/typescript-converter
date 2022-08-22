//using System;
//using System.Collections.Generic;
//using System.Text;

///*
// * Copyright (c) 1999, 2013, Oracle and/or its affiliates. All rights reserved.
// * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
// *
// * This code is free software; you can redistribute it and/or modify it
// * under the terms of the GNU General Public License version 2 only, as
// * published by the Free Software Foundation.  Oracle designates this
// * particular file as subject to the "Classpath" exception as provided
// * by Oracle in the LICENSE file that accompanied this code.
// *
// * This code is distributed in the hope that it will be useful, but WITHOUT
// * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
// * version 2 for more details (a copy is included in the LICENSE file that
// * accompanied this code).
// *
// * You should have received a copy of the GNU General Public License version
// * 2 along with this work; if not, write to the Free Software Foundation,
// * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
// *
// * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
// * or visit www.oracle.com if you need additional information or have any
// * questions.
// */

//namespace com.sun.tools.javac.util
//{

//    /// <summary>
//    /// A class for generic linked lists. Links are supposed to be
//    ///  immutable, the only exception being the incremental construction of
//    ///  lists via ListBuffers.  List is the main container class in
//    ///  GJC. Most data structures and algorithms in GJC use lists rather
//    ///  than arrays.
//    /// 
//    ///  <para>Lists are always trailed by a sentinel element, whose head and tail
//    ///  are both null.
//    /// 
//    /// </para>
//    ///  <para><b>This is NOT part of any supported API.
//    ///  If you write code that depends on this, you do so at your own risk.
//    ///  This code and its internal interfaces are subject to change or
//    ///  deletion without notice.</b>
//    /// </para>
//    /// </summary>
//    public class List<A> : AbstractCollection<A>, IList<A>
//    {

//        /// <summary>
//        /// The first element of the list, supposed to be immutable.
//        /// </summary>
//        public A head;

//        /// <summary>
//        /// The remainder of the list except for its first element, supposed
//        ///  to be immutable.
//        /// </summary>
//        //@Deprecated
//        public List<A> tail;

//        /// <summary>
//        /// Construct a list given its head and tail.
//        /// </summary>
//        internal List(A head, List<A> tail)
//        {
//            this.tail = tail;
//            this.head = head;
//        }

//        /// <summary>
//        /// Construct an empty list.
//        /// </summary>
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings("unchecked") public static <A> List<A> nil()
//        public static List<A> nil<A>()
//        {
//            return (List<A>)EMPTY_LIST;
//        }

////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: private static final List<?> EMPTY_LIST = new List<Object>(null,null)
//        private static readonly List<object> EMPTY_LIST = new ListAnonymousInnerClass();

//        private class ListAnonymousInnerClass : List<object>
//        {
//            public ListAnonymousInnerClass() : base(null, null)
//            {
//            }

//            public virtual List<object> setTail(List<object> tail)
//            {
//                throw new System.NotSupportedException();
//            }
//            public virtual bool isEmpty()
//            {
//                return true;
//            }
//        }

//        /// <summary>
//        /// Returns the list obtained from 'l' after removing all elements 'elem'
//        /// </summary>
//        public static List<A> filter<A>(List<A> l, A elem)
//        {
//            Assert.checkNonNull(elem);
//            List<A> res = List.nil();
//            foreach (A a in l)
//            {
//                if (a != default(A) && !a.Equals(elem))
//                {
//                    res = res.prepend(a);
//                }
//            }
//            return res.reverse();
//        }

//        public virtual List<A> intersect(List<A> that)
//        {
//            ListBuffer<A> buf = new ListBuffer<A>();
//            foreach (A el in this)
//            {
//                if (that.Contains(el))
//                {
//                    buf.append(el);
//                }
//            }
//            return buf.toList();
//        }

//        public virtual List<A> diff(List<A> that)
//        {
//            ListBuffer<A> buf = new ListBuffer<A>();
//            foreach (A el in this)
//            {
//                if (!that.Contains(el))
//                {
//                    buf.append(el);
//                }
//            }
//            return buf.toList();
//        }

//        /// <summary>
//        /// Create a new list from the first {@code n} elements of this list
//        /// </summary>
//        public virtual List<A> take(int n)
//        {
//            ListBuffer<A> buf = new ListBuffer<A>();
//            int count = 0;
//            foreach (A el in this)
//            {
//                if (count++ == n)
//                {
//                    break;
//                }
//                buf.append(el);
//            }
//            return buf.toList();
//        }

//        /// <summary>
//        /// Construct a list consisting of given element.
//        /// </summary>
//        public static List<A> of<A>(A x1)
//        {
//            return new List<A>(x1, List.nil());
//        }

//        /// <summary>
//        /// Construct a list consisting of given elements.
//        /// </summary>
//        public static List<A> of<A>(A x1, A x2)
//        {
//            return new List<A>(x1, of(x2));
//        }

//        /// <summary>
//        /// Construct a list consisting of given elements.
//        /// </summary>
//        public static List<A> of<A>(A x1, A x2, A x3)
//        {
//            return new List<A>(x1, of(x2, x3));
//        }

//        /// <summary>
//        /// Construct a list consisting of given elements.
//        /// </summary>
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings({"varargs", "unchecked"}) public static <A> List<A> of(A x1, A x2, A x3, A... rest)
//        public static List<A> of<A>(A x1, A x2, A x3, params A[] rest)
//        {
//            return new List<A>(x1, new List<A>(x2, new List<A>(x3, from(rest))));
//        }

//        /// <summary>
//        /// Construct a list consisting all elements of given array. </summary>
//        /// <param name="array"> an array; if {@code null} return an empty list </param>
//        public static List<A> from<A>(A[] array)
//        {
//            List<A> xs = nil();
//            if (array != null)
//            {
//                for (int i = array.Length - 1; i >= 0; i--)
//                {
//                    xs = new List<>(array[i], xs);
//                }
//            }
//            return xs;
//        }

//        public static List<A> from<A, T1>(IEnumerable<T1> coll) where T1 : A
//        {
//            ListBuffer<A> xs = new ListBuffer<A>();
//            foreach (A a in coll)
//            {
//                xs.append(a);
//            }
//            return xs.toList();
//        }

//        /// <summary>
//        /// Construct a list consisting of a given number of identical elements. </summary>
//        ///  <param name="len">    The number of elements in the list. </param>
//        ///  <param name="init">   The value of each element. </param>
//        [Obsolete]
//        public static List<A> fill<A>(int len, A init)
//        {
//            List<A> l = nil();
//            for (int i = 0; i < len; i++)
//            {
//                l = new List<>(init, l);
//            }
//            return l;
//        }

//        /// <summary>
//        /// Does list have no elements?
//        /// </summary>
//        public override bool isEmpty()
//        {
//            return tail == null;
//        }

//        /// <summary>
//        /// Does list have elements?
//        /// </summary>
//        //@Deprecated
//        public virtual bool nonEmpty()
//        {
//            return tail != null;
//        }

//        /// <summary>
//        /// Return the number of elements in this list.
//        /// </summary>
//        //@Deprecated
//        public virtual int length()
//        {
//            List<A> l = this;
//            int len = 0;
//            while (l.tail != null)
//            {
//                l = l.tail;
//                len++;
//            }
//            return len;
//        }
//        public virtual int getCount()
//        {
//            return length();
//        }

//        public virtual List<A> setTail(List<A> tail)
//        {
//            this.tail = tail;
//            return tail;
//        }

//        /// <summary>
//        /// Prepend given element to front of list, forming and returning
//        ///  a new list.
//        /// </summary>
//        public virtual List<A> prepend(A x)
//        {
//            return new List<A>(x, this);
//        }

//        /// <summary>
//        /// Prepend given list of elements to front of list, forming and returning
//        ///  a new list.
//        /// </summary>
//        public virtual List<A> prependList(List<A> xs)
//        {
//            if (this.isEmpty())
//            {
//                return xs;
//            }
//            if (xs.Count == 0)
//            {
//                return this;
//            }
//            if (xs.tail.Count == 0)
//            {
//                return prepend(xs.head);
//            }
//            // return this.prependList(xs.tail).prepend(xs.head);
//            List<A> result = this;
//            List<A> rev = xs.reverse();
//            Assert.check(rev != xs);
//            // since xs.reverse() returned a new list, we can reuse the
//            // individual List objects, instead of allocating new ones.
//            while (rev.nonEmpty())
//            {
//                List<A> h = rev;
//                rev = rev.tail;
//                h.setTail(result);
//                result = h;
//            }
//            return result;
//        }

//        /// <summary>
//        /// Reverse list.
//        /// If the list is empty or a singleton, then the same list is returned.
//        /// Otherwise a new list is formed.
//        /// </summary>
//        public virtual List<A> reverse()
//        {
//            // if it is empty or a singleton, return itself
//            if (isEmpty() || tail.Count == 0)
//            {
//                return this;
//            }

//            List<A> rev = nil();
//            for (List<A> l = this; l.nonEmpty(); l = l.tail)
//            {
//                rev = new List<>(l.head, rev);
//            }
//            return rev;
//        }

//        /// <summary>
//        /// Append given element at length, forming and returning
//        ///  a new list.
//        /// </summary>
//        public virtual List<A> append(A x)
//        {
//            return of(x).prependList(this);
//        }

//        /// <summary>
//        /// Append given list at length, forming and returning
//        ///  a new list.
//        /// </summary>
//        public virtual List<A> appendList(List<A> x)
//        {
//            return x.prependList(this);
//        }

//        /// <summary>
//        /// Append given list buffer at length, forming and returning a new
//        /// list.
//        /// </summary>
//        public virtual List<A> appendList(ListBuffer<A> x)
//        {
//            return appendList(x.toList());
//        }

//        /// <summary>
//        /// Copy successive elements of this list into given vector until
//        ///  list is exhausted or end of vector is reached.
//        /// </summary>
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T[] toArray(T[] vec)
//        public override T[] toArray<T>(T[] vec)
//        {
//            int i = 0;
//            List<A> l = this;
//            object[] dest = vec;
//            while (l.nonEmpty() && i < vec.Length)
//            {
//                dest[i] = l.head;
//                l = l.tail;
//                i++;
//            }
//            if (l.Count == 0)
//            {
//                if (i < vec.Length)
//                {
//                    vec[i] = default(T);
//                }
//                return vec;
//            }

//            vec = (T[])Array.CreateInstance(vec.GetType().GetElementType(), size());
//            return toArray(vec);
//        }

//        public virtual object[] toArray()
//        {
//            return toArray(new object[size()]);
//        }

//        /// <summary>
//        /// Form a string listing all elements with given separator character.
//        /// </summary>
//        public virtual string ToString(string sep)
//        {
//            if (isEmpty())
//            {
//                return "";
//            }
//            else
//            {
//                StringBuilder buf = new StringBuilder();
//                buf.Append(head);
//                for (List<A> l = tail; l.nonEmpty(); l = l.tail)
//                {
//                    buf.Append(sep);
//                    buf.Append(l.head);
//                }
//                return buf.ToString();
//            }
//        }

//        /// <summary>
//        /// Form a string listing all elements with comma as the separator character.
//        /// </summary>
//        public override string ToString()
//        {
//            return ToString(",");
//        }

//        /// <summary>
//        /// Compute a hash code, overrides Object </summary>
//        ///  <seealso cref= java.util.List#hashCode </seealso>
//        public override int GetHashCode()
//        {
//            List<A> l = this;
//            int h = 1;
//            while (l.tail != null)
//            {
//                h = h * 31 + (l.head == default(A) ? 0 : l.head.GetHashCode());
//                l = l.tail;
//            }
//            return h;
//        }

//        /// <summary>
//        /// Is this list the same as other list? </summary>
//        ///  <seealso cref= java.util.List#equals </seealso>
//        public override bool Equals(object other)
//        {
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: if (other instanceof List<?>)
//            if (other is List<object>)
//            {
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: return equals(this, (List<?>)other);
//                return Equals(this, (List<object>)other);
//            }
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: if (other instanceof java.util.List<?>)
//            if (other is IList<object>)
//            {
//                List<A> t = this;
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: java.util.Iterator<?> oIter = ((java.util.List<?>) other).iterator();
//                IEnumerator<object> oIter = ((IList<object>) other).GetEnumerator();
//                while (t.tail != null && oIter.MoveNext())
//                {
//                    object o = oIter.Current;
//                    if (!(t.head == default(A) ? o == null : t.head.Equals(o)))
//                    {
//                        return false;
//                    }
//                    t = t.tail;
//                }
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
//                return (t.Count == 0 && !oIter.hasNext());
//            }
//            return false;
//        }

//        /// <summary>
//        /// Are the two lists the same?
//        /// </summary>
//        public static bool Equals<T1, T2>(List<T1> xs, List<T2> ys)
//        {
//            while (xs.tail != null && ys.tail != null)
//            {
//                if (xs.head == default(T1))
//                {
//                    if (ys.head != default(T2))
//                    {
//                        return false;
//                    }
//                }
//                else
//                {
//                    if (!xs.head.Equals(ys.head))
//                    {
//                        return false;
//                    }
//                }
//                xs = xs.tail;
//                ys = ys.tail;
//            }
//            return xs.tail == null && ys.tail == null;
//        }

//        /// <summary>
//        /// Does the list contain the specified element?
//        /// </summary>
//        public virtual bool Contains(object x)
//        {
//            List<A> l = this;
//            while (l.tail != null)
//            {
//                if (x == null)
//                {
//                    if (l.head == default(A))
//                    {
//                        return true;
//                    }
//                }
//                else
//                {
//                    if (l.head.Equals(x))
//                    {
//                        return true;
//                    }
//                }
//                l = l.tail;
//            }
//            return false;
//        }

//        /// <summary>
//        /// The last element in the list, if any, or null.
//        /// </summary>
//        public virtual A last()
//        {
//            A last = default(A);
//            List<A> t = this;
//            while (t.tail != null)
//            {
//                last = t.head;
//                t = t.tail;
//            }
//            return last;
//        }

////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings("unchecked") public <Z> List<Z> map(System.Func<A, Z> mapper)
//        public virtual List<Z> map<Z>(System.Func<A, Z> mapper)
//        {
//            bool changed = false;
//            ListBuffer<Z> buf = new ListBuffer<Z>();
//            foreach (A a in this)
//            {
//                Z z = mapper(a);
//                buf.append(z);
//                changed |= (z != a);
//            }
//            return changed ? buf.toList() : (List<Z>)this;
//        }

////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings("unchecked") public static <T> List<T> convert(Class<T> klass, List<?> list)
//        public static List<T> convert<T, T1>(Type<T> klass, List<T1> list)
//        {
//            if (list == null)
//            {
//                return null;
//            }
//            foreach (object o in list)
//            {
//                klass.cast(o);
//            }
//            return (List<T>)list;
//        }

////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
////ORIGINAL LINE: private static final java.util.Iterator<?> EMPTYITERATOR = new java.util.Iterator<Object>()
//        private static readonly IEnumerator<object> EMPTYITERATOR = new IteratorAnonymousInnerClass();

//        private class IteratorAnonymousInnerClass : IEnumerator<object>
//        {
//            public IteratorAnonymousInnerClass()
//            {
//            }

//            public virtual bool hasNext()
//            {
//                return false;
//            }
//            public virtual object next()
//            {
//                throw new NoSuchElementException();
//            }
//            public virtual void remove()
//            {
//                throw new System.NotSupportedException();
//            }
//        }

////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings("unchecked") private static <A> java.util.Iterator<A> emptyIterator()
//        private static IEnumerator<A> emptyIterator<A>()
//        {
//            return (IEnumerator<A>)EMPTYITERATOR;
//        }

//        public virtual IEnumerator<A> GetEnumerator()
//        {
//            if (tail == null)
//            {
//                return emptyIterator();
//            }
//            return new IteratorAnonymousInnerClass2(this);
//        }

//        private class IteratorAnonymousInnerClass2 : IEnumerator<A>
//        {
//            private readonly List<A> outerInstance;

//            public IteratorAnonymousInnerClass2(List<A> outerInstance)
//            {
//                this.outerInstance = outerInstance;
//                elems = outerInstance;
//            }

//            internal List<A> elems;
//            public virtual bool hasNext()
//            {
//                return elems.tail != null;
//            }
//            public virtual A next()
//            {
//                if (elems.tail == null)
//                {
//                    throw new NoSuchElementException();
//                }
//                A result = elems.head;
//                elems = elems.tail;
//                return result;
//            }
//            public virtual void remove()
//            {
//                throw new System.NotSupportedException();
//            }
//        }

//        public virtual A getJavaToDotNetIndexer(int index)
//        {
//            if (index < 0)
//            {
//                throw new System.IndexOutOfRangeException(index.ToString());
//            }

//            List<A> l = this;
//            for (int i = index; i-- > 0 && l.Count > 0; l = l.tail)
//            {
//                ;
//            }

//            if (l.Count == 0)
//            {
//                throw new System.IndexOutOfRangeException("Index: " + index + ", " + "Size: " + size());
//            }
//            return l.head;
//        }

//        public virtual bool addAll<T1>(int index, ICollection<T1> c) where T1 : A
//        {
//            if (c.Count == 0)
//            {
//                return false;
//            }
//            throw new System.NotSupportedException();
//        }

//        public virtual A setJavaToDotNetIndexer(int index, A element)
//        {
//            throw new System.NotSupportedException();
//        }

//        public virtual void Insert(int index, A element)
//        {
//            throw new System.NotSupportedException();
//        }

//        public virtual A remove(int index)
//        {
//            throw new System.NotSupportedException();
//        }

//        public virtual int IndexOf(object o)
//        {
//            int i = 0;
//            for (List<A> l = this; l.tail != null; l = l.tail, i++)
//            {
//                if (l.head == default(A) ? o == null : l.head.Equals(o))
//                {
//                    return i;
//                }
//            }
//            return -1;
//        }

//        public virtual int lastIndexOf(object o)
//        {
//            int last = -1;
//            int i = 0;
//            for (List<A> l = this; l.tail != null; l = l.tail, i++)
//            {
//                if (l.head == default(A) ? o == null : l.head.Equals(o))
//                {
//                    last = i;
//                }
//            }
//            return last;
//        }

//        public virtual IEnumerator<A> listIterator()
//        {
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
//            return Collections.unmodifiableList(new List<A>(this)).GetEnumerator();
//        }

//        public virtual IEnumerator<A> listIterator(int index)
//        {
//            return Collections.unmodifiableList(new List<A>(this)).listIterator(index);
//        }

//        public virtual IList<A> subList(int fromIndex, int toIndex)
//        {
//            if (fromIndex < 0 || toIndex > size() || fromIndex > toIndex)
//            {
//                throw new System.ArgumentException();
//            }

//            List<A> a = new List<A>(toIndex - fromIndex);
//            int i = 0;
//            for (List<A> l = this; l.tail != null; l = l.tail, i++)
//            {
//                if (i == toIndex)
//                {
//                    break;
//                }
//                if (i >= fromIndex)
//                {
//                    a.Add(l.head);
//                }
//            }

//            return Collections.unmodifiableList(a);
//        }

//        /// <summary>
//        /// Collect elements into a new list (using a @code{ListBuffer})
//        /// </summary>
//        public static Collector<Z, ListBuffer<Z>, List<Z>> collector<Z>()
//        {
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Method reference constructor syntax is not converted by JAVA to C# Converter Cracked By X-Cracker:
////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Method reference arbitrary object instance method syntax is not converted by JAVA to C# Converter Cracked By X-Cracker:
//            return Collector.of(ListBuffer::new, ListBuffer::add, (buf1, buf2)=>
//        {
//            buf1.addAll(buf2);
//            return buf1;
//        }, ListBuffer::toList);
//        }
//    }

//}