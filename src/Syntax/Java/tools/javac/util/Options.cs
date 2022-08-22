using System.Collections.Generic;
using System.Threading;

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
    // using Option =com.sun.tools.javac.main.Option;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import staticcom.sun.tools.javac.main.Option.*;

    /// <summary>
    /// A table of all command-line options.
    ///  If an option has an argument, the option name is mapped to the argument.
    ///  If a set option has no argument, it is mapped to itself.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Options
    {
        private const long serialVersionUID = 0;

        /// <summary>
        /// The context key for the options. </summary>
        public static readonly Context.Key<Options> optionsKey = new Context.Key<Options>();

        private IDictionary<string, string> values;

        /// <summary>
        /// Get the Options instance for this context. </summary>
        public static Options instance(Context context)
        {
            Options instance = context.get(optionsKey);
            if (instance == null)
            {
                instance = new Options(context);
            }
            return instance;
        }

        protected internal Options(Context context)
        {
            // DEBUGGING -- Use LinkedHashMap for reproducibility
            values = new Dictionary<string, string>();
            context.put(optionsKey, this);
        }

        /// <summary>
        /// Get the value for an undocumented option.
        /// </summary>
        public virtual string get(string name)
        {
            return values[name];
        }

        ///// <summary>
        ///// Get the value for an option.
        ///// </summary>
        //public virtual string get(Option option)
        //{
        //    return values.get(option.primaryName);
        //}

        /// <summary>
        /// Get the boolean value for an option, patterned after Boolean.getBoolean,
        /// essentially will return true, iff the value exists and is set to "true".
        /// </summary>
        public virtual bool getBoolean(string name)
        {
            return getBoolean(name, false);
        }

        /// <summary>
        /// Get the boolean with a default value if the option is not set.
        /// </summary>
        public virtual bool getBoolean(string name, bool defaultValue)
        {
            string value = get(name);
            return (string.ReferenceEquals(value, null)) ? defaultValue : bool.Parse(value);
        }

        /// <summary>
        /// Check if the value for an undocumented option has been set.
        /// </summary>
        public virtual bool isSet(string name)
        {
            // return (values[name] != null);
            return values.ContainsKey(name);
        }

        ///// <summary>
        ///// Check if the value for an option has been set.
        ///// </summary>
        //public virtual bool isSet(Option option)
        //{
        //    return (values.get(option.primaryName) != null);
        //}

        ///// <summary>
        ///// Check if the value for a choice option has been set to a specific value.
        ///// </summary>
        //public virtual bool isSet(Option option, string value)
        //{
        //    return (values.get(option.primaryName + value) != null);
        //}

        ///// <summary>
        ///// Check if the value for a lint option has been explicitly set, either with -Xlint:opt
        /////  or if all lint options have enabled and this one not disabled with -Xlint:-opt.
        ///// </summary>
        //public virtual bool isLintSet(string s)
        //{
        //    // return true if either the specific option is enabled, or
        //    // they are all enabled without the specific one being
        //    // disabled
        //    return isSet(XLINT_CUSTOM, s) || (isSet(XLINT) || isSet(XLINT_CUSTOM, "all")) && isUnset(XLINT_CUSTOM, "-" + s);
        //}

        /// <summary>
        /// Check if the value for an undocumented option has not been set.
        /// </summary>
        public virtual bool isUnset(string name)
        {
            return (values[name] == null);
        }

        ///// <summary>
        ///// Check if the value for an option has not been set.
        ///// </summary>
        //public virtual bool isUnset(Option option)
        //{
        //    return (values.get(option.primaryName) == null);
        //}

        ///// <summary>
        ///// Check if the value for a choice option has not been set to a specific value.
        ///// </summary>
        //public virtual bool isUnset(Option option, string value)
        //{
        //    return (values.get(option.primaryName + value) == null);
        //}

        public virtual void put(string name, string value)
        {
            values.Add(name, value);
        }

        //public virtual void put(Option option, string value)
        //{
        //    values.put(option.primaryName, value);
        //}

        //public virtual void putAll(Options options)
        //{
        //    values.putAll(options.values);
        //}

        public virtual void remove(string name)
        {
            values.Remove(name);
        }

        public virtual ICollection<string> keySet()
        {
            return values.Keys;
        }

        public virtual int size()
        {
            return values.Count;
        }

        //// light-weight notification mechanism

        //private List<ThreadStart> listeners = List.nil();

        //public virtual void addListener(ThreadStart listener)
        //{
        //    listeners = listeners.prepend(listener);
        //}

        //public virtual void notifyListeners()
        //{
        //    foreach (ThreadStart r in listeners)
        //    {
        //        r.run();
        //    }
        //}

        public virtual void clear()
        {
            //values.clear();
            //listeners = List.nil();
            values.Clear();
        }
    }

}