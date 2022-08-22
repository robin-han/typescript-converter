using System.Collections.Generic;

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

namespace com.sun.source.tree
{
    using java.lang.common.api;

    /// <summary>
    /// A tree node for a member reference expression.
    /// 
    /// For example:
    /// <pre>
    ///   <em>expression</em> # <em>[ identifier | new ]</em>
    /// </pre>
    /// 
    /// @since 1.8
    /// </summary>
    public interface MemberReferenceTree : ExpressionTree
    {

        /// <summary>
        /// There are two kinds of member references: (i) method references and
        /// (ii) constructor references
        /// </summary>

        /// <summary>
        /// Returns the mode of the reference. </summary>
        /// <returns> the mode </returns>
        ReferenceMode getMode();

        /// <summary>
        /// Returns the qualifier expression for the reference. </summary>
        /// <returns> the qualifier expression </returns>
        ExpressionTree getQualifierExpression();

        /// <summary>
        /// Returns the name of the reference. </summary>
        /// <returns> the name </returns>
        Name getName();

        /// <summary>
        /// Returns the type arguments for the reference. </summary>
        /// <returns> the type arguments </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends ExpressionTree> getTypeArguments();
        IList<ExpressionTree> getTypeArguments();
    }

    public enum ReferenceMode
    {
        /// <summary>
        /// enum constant for method references. </summary>
        INVOKE,
        /// <summary>
        /// enum constant for constructor references. </summary>
        NEW
    }

}