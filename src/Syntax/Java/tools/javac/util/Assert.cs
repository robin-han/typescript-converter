/*
 * Copyright (c) 2011, 2014, Oracle and/or its affiliates. All rights reserved.
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
    /// Simple facility for unconditional assertions.
    /// The methods in this class are described in terms of equivalent assert
    /// statements, assuming that assertions have been enabled.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Assert
    {
        /// <summary>
        /// Equivalent to
        ///   assert cond;
        /// </summary>
        public static void check(bool cond)
        {
            if (!cond)
            {
                error();
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o == null);
        /// </summary>
        public static void checkNull(object o)
        {
            if (o != null)
            {
                error();
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (t != null); return t;
        /// </summary>
        public static T checkNonNull<T>(T t)
        {
            if (t == null)
            {
                error();
            }
            return t;
        }

        /// <summary>
        /// Equivalent to
        ///   assert cond : value;
        /// </summary>
        public static void check(bool cond, int value)
        {
            if (!cond)
            {
                error(value.ToString());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert cond : value;
        /// </summary>
        public static void check(bool cond, long value)
        {
            if (!cond)
            {
                error(value.ToString());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert cond : value;
        /// </summary>
        public static void check(bool cond, object value)
        {
            if (!cond)
            {
                error(value.ToString());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert cond : msg;
        /// </summary>
        public static void check(bool cond, string msg)
        {
            if (!cond)
            {
                error(msg);
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert cond : msg.get();
        ///  Note: message string is computed lazily.
        /// </summary>
        public static void check(bool cond, System.Func<string> msg)
        {
            if (!cond)
            {
                error(msg());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o == null) : value;
        /// </summary>
        public static void checkNull(object o, object value)
        {
            if (o != null)
            {
                error(value.ToString());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o == null) : msg;
        /// </summary>
        public static void checkNull(object o, string msg)
        {
            if (o != null)
            {
                error(msg);
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o == null) : msg.get();
        ///  Note: message string is computed lazily.
        /// </summary>
        public static void checkNull(object o, System.Func<string> msg)
        {
            if (o != null)
            {
                error(msg());
            }
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o != null) : msg;
        /// </summary>
        public static T checkNonNull<T>(T t, string msg)
        {
            if (t == null)
            {
                error(msg);
            }
            return t;
        }

        /// <summary>
        /// Equivalent to
        ///   assert (o != null) : msg.get();
        ///  Note: message string is computed lazily.
        /// </summary>
        public static T checkNonNull<T>(T t, System.Func<string> msg)
        {
            if (t == null)
            {
                error(msg());
            }
            return t;
        }

        /// <summary>
        /// Equivalent to
        ///   assert false;
        /// </summary>
        public static void error()
        {
            throw new AssertionError();
        }

        /// <summary>
        /// Equivalent to
        ///   assert false : msg;
        /// </summary>
        public static void error(string msg)
        {
            throw new AssertionError(msg);
        }

        /// <summary>
        /// Prevent instantiation. </summary>
        private Assert()
        {
        }
    }

}