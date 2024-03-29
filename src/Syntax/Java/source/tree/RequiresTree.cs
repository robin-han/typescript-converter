﻿/*
 * Copyright (c) 2009, 2016, Oracle and/or its affiliates. All rights reserved.
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

namespace com.sun.source.tree
{
    /// <summary>
    /// A tree node for a 'requires' directive in a module declaration.
    /// 
    /// For example:
    /// <pre>
    ///    requires <em>module-name</em>;
    ///    requires static <em>module-name</em>;
    ///    requires transitive <em>module-name</em>;
    /// </pre>
    /// 
    /// @since 9
    /// </summary>
    public interface RequiresTree : DirectiveTree
    {
        /// <summary>
        /// Returns true if this is a "requires static" directive. </summary>
        /// <returns> true if this is a "requires static" directive </returns>
        bool isStatic();

        /// <summary>
        /// Returns true if this is a "requires transitive" directive. </summary>
        /// <returns> true if this is a "requires transitive" directive </returns>
        bool isTransitive();

        /// <summary>
        /// Returns the name of the module that is required. </summary>
        /// <returns> the name of the module that is required </returns>
        ExpressionTree getModuleName();
    }

}