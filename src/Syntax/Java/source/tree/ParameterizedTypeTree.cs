﻿using System.Collections.Generic;

/*
 * Copyright (c) 2005, 2014, Oracle and/or its affiliates. All rights reserved.
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
    /// A tree node for a type expression involving type parameters.
    /// 
    /// For example:
    /// <pre>
    ///   <em>type</em> &lt; <em>typeArguments</em> &gt;
    /// </pre>
    /// 
    /// @jls 4.5.1 Type Arguments of Parameterized Types
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// @since 1.6
    /// </summary>
    public interface ParameterizedTypeTree : Tree
    {
        /// <summary>
        /// Returns the base type. </summary>
        /// <returns> the base type </returns>
        Tree getType();

        /// <summary>
        /// Returns the type arguments. </summary>
        /// <returns> the type arguments </returns>
//JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<? extends Tree> getTypeArguments();
        IList<Tree> getTypeArguments();
    }

}