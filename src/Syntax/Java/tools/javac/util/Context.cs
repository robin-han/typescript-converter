using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using System;
using System.Collections.Generic;
using System.Collections;
/*
 * Copyright (c) 2001, 2019, Oracle and/or its affiliates. All rights reserved.
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
    using java.lang.common.api;

    /// <summary>
    /// Support for an abstract context, modelled loosely after ThreadLocal
    /// but using a user-provided context instead of the current thread.
    /// 
    /// <para>Within the compiler, a single Context is used for each
    /// invocation of the compiler.  The context is then used to ensure a
    /// single copy of each compiler phase exists per compiler invocation.
    /// 
    /// </para>
    /// <para>The context can be used to assist in extending the compiler by
    /// extending its components.  To do that, the extended component must
    /// be registered before the base component.  We break initialization
    /// cycles by (1) registering a factory for the component rather than
    /// the component itself, and (2) a convention for a pattern of usage
    /// in which each base component registers itself by calling an
    /// instance method that is overridden in extended components.  A base
    /// phase supporting extension would look something like this:
    /// 
    /// <pre>{@code
    /// public class Phase {
    ///     protected static final Context.Key<Phase> phaseKey =
    ///         new Context.Key<Phase>();
    /// 
    ///     public static Phase instance(Context context) {
    ///         Phase instance = context.get(phaseKey);
    ///         if (instance == null)
    ///             // the phase has not been overridden
    ///             instance = new Phase(context);
    ///         return instance;
    ///     }
    /// 
    ///     protected Phase(Context context) {
    ///         context.put(phaseKey, this);
    ///         // other initialization follows...
    ///     }
    /// }
    /// }</pre>
    /// 
    /// </para>
    /// <para>In the compiler, we simply use Phase.instance(context) to get
    /// the reference to the phase.  But in extensions of the compiler, we
    /// must register extensions of the phases to replace the base phase,
    /// and this must be done before any reference to the phase is accessed
    /// using Phase.instance().  An extended phase might be declared thus:
    /// 
    /// <pre>{@code
    /// public class NewPhase extends Phase {
    ///     protected NewPhase(Context context) {
    ///         super(context);
    ///     }
    ///     public static void preRegister(final Context context) {
    ///         context.put(phaseKey, new Context.Factory<Phase>() {
    ///             public Phase make() {
    ///                 return new NewPhase(context);
    ///             }
    ///         });
    ///     }
    /// }
    /// }</pre>
    /// 
    /// </para>
    /// <para>And is registered early in the extended compiler like this
    /// 
    /// <pre>
    ///     NewPhase.preRegister(context);
    /// </pre>
    /// 
    /// </para>
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Context
    {
        /// <summary>
        /// The client creates an instance of this class for each key.
        /// </summary>
        public class Key<T>
        {
            // note: we inherit identity equality from Object.
        }

        /// <summary>
        /// The client can register a factory for lazy creation of the
        /// instance.
        /// </summary>
        public interface Factory<T>
        {
            T make(Context c);
        }

        /// <summary>
        /// The underlying map storing the data.
        /// We maintain the invariant that this table contains only
        /// mappings of the form
        /// {@literal Key<T> -> T }
        /// or
        /// {@literal Key<T> -> Factory<T> }
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: protected final Map<Key<?>,Object> ht = new HashMap<>();
        protected internal readonly Hashtable ht = new Hashtable();

        /// <summary>
        /// Set the factory for the key in this context. </summary>
        public virtual void put<T>(Key<T> key, Factory<T> fac)
        {
            checkState(ht);
            object old = ht[key] = fac;
            if (old != null)
            {
                throw new AssertionError("duplicate context value");
            }
            checkState(ft);
            ft[key] = fac; // cannot be duplicate if unique in ht
        }

        /// <summary>
        /// Set the value for the key in this context. </summary>
        public virtual void put<T>(Key<T> key, T data)
        {
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            //ORIGINAL LINE: if (data instanceof Factory<?>)
            if (data is Factory<object>)
            {
                throw new AssertionError("T extends Context.Factory");
            }
            checkState(ht);
            object old = ht[key] = data;
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            //ORIGINAL LINE: if (old != null && !(old instanceof Factory<?>) && old != data && data != null)
            if (old != null && !(old is Factory<T>) && old != (object)data && data != null)
            {
                throw new AssertionError("duplicate context value");
            }
        }

        /// <summary>
        /// Get the value for the key in this context. </summary>
        public virtual T get<T>(Key<T> key)
        {
            checkState(ht);
            object o = ht[key];
            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
            //ORIGINAL LINE: if (o instanceof Factory<?>)
            if (o is Factory<object>)
            {
                //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
                //ORIGINAL LINE: Factory<?> fac = (Factory<?>)o;
                Factory<object> fac = (Factory<object>)o;
                o = fac.make(this);
                //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
                //ORIGINAL LINE: if (o instanceof Factory<?>)
                if (o is Factory<object>)
                {
                    throw new AssertionError("T extends Context.Factory");
                }
                Assert.check(ht[key] == o);
            }

            /* The following cast can't fail unless there was
             * cheating elsewhere, because of the invariant on ht.
             * Since we found a key of type Key<T>, the value must
             * be of type T.
             */
            return Context.uncheckedCast<T>(o);
        }

        public Context()
        {
        }

        /// <summary>
        /// The table of preregistered factories.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: private final Map<Key<?>,Factory<?>> ft = new HashMap<>();
        private readonly IDictionary ft = new Hashtable();

        ///*
        // * The key table, providing a unique Key<T> for each Class<T>.
        // */
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        ////ORIGINAL LINE: private final Map<Class, Key<?>> kt = new HashMap<>();
        //private readonly IDictionary kt = new Hashtable();

        //protected internal virtual Key<T> key<T>(Type<T> clss)
        //{
        //    checkState(kt);
        //    Key<T> k = uncheckedCast(kt[clss]);
        //    if (k == null)
        //    {
        //        k = new Key<>();
        //        kt[clss] = k;
        //    }
        //    return k;
        //}

        //public virtual T get<T>(Type<T> clazz)
        //{
        //    return get(key(clazz));
        //}

        //public virtual void put<T>(Type<T> clazz, T data)
        //{
        //    put(key(clazz), data);
        //}
        //public virtual void put<T>(Type<T> clazz, Factory<T> fac)
        //{
        //    put(key(clazz), fac);
        //}

        /// <summary>
        /// TODO: This method should be removed and Context should be made type safe.
        /// This can be accomplished by using class literals as type tokens.
        /// </summary>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") private static <T> T uncheckedCast(Object o)
        private static T uncheckedCast<T>(object o)
        {
            return (T)o;
        }

        public virtual void dump()
        {
            foreach (object value in ht.Values)
            {
                Console.Error.WriteLine(value == null ? null : value.GetType());
            }
        }

        private static void checkState(IDictionary t)
        {
            if (t == null)
            {
                throw new System.InvalidOperationException();
            }
        }
    }

}