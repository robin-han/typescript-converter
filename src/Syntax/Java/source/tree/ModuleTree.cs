using System.Collections.Generic;

/*
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
    /// A tree node for a module declaration.
    /// 
    /// For example:
    /// <pre>
    ///    <em>annotations</em>
    ///    [open] module <em>module-name</em> {
    ///        <em>directives</em>
    ///    }
    /// </pre>
    /// 
    /// @since 9
    /// </summary>
    public interface ModuleTree : Tree
    {
        /// <summary>
        /// Returns the annotations associated with this module declaration. </summary>
        /// <returns> the annotations </returns>
//JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<? extends AnnotationTree> getAnnotations();
        IList<AnnotationTree> getAnnotations();

        /// <summary>
        /// Returns the type of this module. </summary>
        /// <returns> the type of this module </returns>
        ModuleKind getModuleType();

        /// <summary>
        /// Returns the name of the module. </summary>
        /// <returns> the name of the module </returns>
        ExpressionTree getName();

        /// <summary>
        /// Returns the directives in the module declaration. </summary>
        /// <returns> the directives in the module declaration </returns>
//JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<? extends DirectiveTree> getDirectives();
        IList<DirectiveTree> getDirectives();

        /// <summary>
        /// The kind of the module.
        /// </summary>

    }

    public enum ModuleKind
    {
        /// <summary>
        /// Open module.
        /// </summary>
        OPEN,
        /// <summary>
        /// Strong module.
        /// </summary>
        STRONG
    }

}