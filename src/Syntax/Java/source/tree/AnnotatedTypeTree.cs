using System.Collections.Generic;

/*
 * Copyright (c) 2008, 2014, Oracle and/or its affiliates. All rights reserved.
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
    /// A tree node for an annotated type.
    /// 
    /// For example:
    /// <pre>
    ///    {@code @}<em>annotationType String</em>
    ///    {@code @}<em>annotationType</em> ( <em>arguments</em> ) <em>Date</em>
    /// </pre>
    /// </summary>
    /// <seealso cref= "JSR 308: Annotations on Java Types"
    /// 
    /// @author Mahmood Ali
    /// @since 1.8 </seealso>
    public interface AnnotatedTypeTree : ExpressionTree
    {
        /// <summary>
        /// Returns the annotations associated with this type expression. </summary>
        /// <returns> the annotations </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends AnnotationTree> getAnnotations();
        IList<AnnotationTree> getAnnotations();

        /// <summary>
        /// Returns the underlying type with which the annotations are associated. </summary>
        /// <returns> the underlying type </returns>
        ExpressionTree getUnderlyingType();
    }

}