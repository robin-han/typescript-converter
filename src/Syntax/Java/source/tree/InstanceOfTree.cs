/*
 * Copyright (c) 2005, 2020, Oracle and/or its affiliates. All rights reserved.
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
    /// A tree node for an {@code instanceof} expression.
    /// 
    /// For example:
    /// <pre>
    ///   <em>expression</em> instanceof <em>type</em>
    /// </pre>
    /// 
    /// @jls 15.20.2 Type Comparison Operator instanceof
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// @since 1.6
    /// </summary>
    public interface InstanceOfTree : ExpressionTree
    {
        /// <summary>
        /// Returns the expression to be tested. </summary>
        /// <returns> the expression </returns>
        ExpressionTree getExpression();

        /// <summary>
        /// Returns the type for which to check. </summary>
        /// <returns> the type </returns>
        /// <seealso cref= #getPattern() </seealso>
        Tree getType();

        /// <summary>
        /// Returns the tested pattern, or null if this instanceof does not use
        /// a pattern.
        /// 
        /// <para>For instanceof with a pattern, i.e. in the following form:
        /// <pre>
        ///   <em>expression</em> instanceof <em>type</em> <em>variable name</em>
        /// </pre>
        /// returns the pattern.
        /// 
        /// </para>
        /// <para>For instanceof without a pattern, i.e. in the following form:
        /// <pre>
        ///   <em>expression</em> instanceof <em>type</em>
        /// </pre>
        /// returns null.
        /// 
        /// </para>
        /// </summary>
        /// <returns> the tested pattern, or null if this instanceof does not use a pattern
        /// @since 16 </returns>
        PatternTree getPattern();
    }

}