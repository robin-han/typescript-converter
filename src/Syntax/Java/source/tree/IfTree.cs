﻿/*
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
    /// A tree node for an {@code if} statement.
    /// 
    /// For example:
    /// <pre>
    ///   if ( <em>condition</em> )
    ///      <em>thenStatement</em>
    /// 
    ///   if ( <em>condition</em> )
    ///       <em>thenStatement</em>
    ///   else
    ///       <em>elseStatement</em>
    /// </pre>
    /// 
    /// @jls 14.9 The if Statement
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// @since 1.6
    /// </summary>
    public interface IfTree : StatementTree
    {
        /// <summary>
        /// Returns the condition of the if-statement. </summary>
        /// <returns> the condition </returns>
        ExpressionTree getCondition();

        /// <summary>
        /// Returns the statement to be executed if the condition is true </summary>
        /// <returns> the statement to be executed if the condition is true </returns>
        StatementTree getThenStatement();

        /// <summary>
        /// Returns the statement to be executed if the condition is false,
        /// or {@code null} if there is no such statement. </summary>
        /// <returns> the statement to be executed if the condition is false </returns>
        StatementTree getElseStatement();
    }

}