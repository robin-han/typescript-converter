using System.Collections.Generic;

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
    using java.lang.common.api;

    /// <summary>
    /// A tree node for a class, interface, enum, record, or annotation
    /// type declaration.
    /// 
    /// For example:
    /// <pre>
    ///   <em>modifiers</em> class <em>simpleName</em> <em>typeParameters</em>
    ///       extends <em>extendsClause</em>
    ///       implements <em>implementsClause</em>
    ///   {
    ///       <em>members</em>
    ///   }
    /// </pre>
    /// 
    /// @jls 8.1 Class Declarations
    /// @jls 8.9 Enum Types
    /// @jls 8.10 Record Types
    /// @jls 9.1 Interface Declarations
    /// @jls 9.6 Annotation Types
    /// 
    /// @author Peter von der Ah&eacute;
    /// @author Jonathan Gibbons
    /// @since 1.6
    /// </summary>
    public interface ClassTree : StatementTree
    {
        /// <summary>
        /// Returns the modifiers, including any annotations,
        /// for this type declaration. </summary>
        /// <returns> the modifiers </returns>
        ModifiersTree getModifiers();

        /// <summary>
        /// Returns the simple name of this type declaration. </summary>
        /// <returns> the simple name </returns>
        Name getSimpleName();

        /// <summary>
        /// Returns any type parameters of this type declaration. </summary>
        /// <returns> the type parameters </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends TypeParameterTree> getTypeParameters();
        IList<TypeParameterTree> getTypeParameters();

        /// <summary>
        /// Returns the supertype of this type declaration,
        /// or {@code null} if none is provided. </summary>
        /// <returns> the supertype </returns>
        Tree getExtendsClause();

        /// <summary>
        /// Returns the interfaces implemented by this type declaration. </summary>
        /// <returns> the interfaces </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends Tree> getImplementsClause();
        IList<Tree> getImplementsClause();

        /// <summary>
        /// {@preview Associated with sealed classes, a preview feature of the Java language.
        /// 
        ///           This method is associated with <i>sealed classes</i>, a preview
        ///           feature of the Java language. Preview features
        ///           may be removed in a future release, or upgraded to permanent
        ///           features of the Java language.}
        /// 
        /// Returns the subclasses permitted by this type declaration.
        /// 
        /// @implSpec this implementation returns an empty list
        /// </summary>
        /// <returns> the subclasses
        /// 
        /// @since 15 </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @jdk.internal.PreviewFeature(feature=jdk.internal.PreviewFeature.Feature.SEALED_CLASSES, essentialAPI=false) default java.util.List<? extends Tree> getPermitsClause()
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: There is no equivalent in C# to Java default interface methods:
        //        default java.util.List<JavaToDotNetGenericWildcard extends Tree> getPermitsClause()
        //    {
        //        return List.of();
        //    }
        IList<Tree> getPermitsClause();

        /// <summary>
        /// Returns the members declared in this type declaration. </summary>
        /// <returns> the members </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends Tree> getMembers();
        IList<Tree> getMembers();
    }

}