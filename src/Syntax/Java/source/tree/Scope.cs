///*
// * Copyright (c) 2006, 2014, Oracle and/or its affiliates. All rights reserved.
// * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
// *
// * This code is free software; you can redistribute it and/or modify it
// * under the terms of the GNU General Public License version 2 only, as
// * published by the Free Software Foundation.  Oracle designates this
// * particular file as subject to the "Classpath" exception as provided
// * by Oracle in the LICENSE file that accompanied this code.
// *
// * This code is distributed in the hope that it will be useful, but WITHOUT
// * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
// * version 2 for more details (a copy is included in the LICENSE file that
// * accompanied this code).
// *
// * You should have received a copy of the GNU General Public License version
// * 2 along with this work; if not, write to the Free Software Foundation,
// * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
// *
// * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
// * or visit www.oracle.com if you need additional information or have any
// * questions.
// */

//namespace com.sun.source.tree
//{

//    /// <summary>
//    /// Interface for determining locally available program elements, such as
//    /// local variables and imports.
//    /// Upon creation, a Scope is associated with a given program position;
//    /// for example, a <seealso cref="Tree tree node"/>. This position may be used to
//    /// infer an enclosing method and/or class.
//    /// 
//    /// <para>A Scope does not itself contain the details of the elements corresponding
//    /// to the parameters, methods and fields of the methods and classes containing
//    /// its position. However, these elements can be determined from the enclosing
//    /// elements.
//    /// 
//    /// </para>
//    /// <para>Scopes may be contained in an enclosing scope. The outermost scope contains
//    /// those elements available via "star import" declarations; the scope within that
//    /// contains the top level elements of the compilation unit, including any named
//    /// imports.
//    /// 
//    /// @since 1.6
//    /// </para>
//    /// </summary>
//    public interface Scope
//    {
//        /// <summary>
//        /// Returns the enclosing scope. </summary>
//        /// <returns> the enclosing scope </returns>
//        Scope getEnclosingScope();

//        /// <summary>
//        /// Returns the innermost type element containing the position of this scope. </summary>
//        /// <returns> the innermost enclosing type element </returns>
//        TypeElement getEnclosingClass();

//        /// <summary>
//        /// Returns the innermost executable element containing the position of this scope. </summary>
//        /// <returns> the innermost enclosing method declaration </returns>
//        ExecutableElement getEnclosingMethod();

//        /// <summary>
//        /// Returns the elements directly contained in this scope. </summary>
//        /// <returns> the elements contained in this scope </returns>
//        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
//        //ORIGINAL LINE: public Iterable<? extends javax.lang.model.element.Element> getLocalElements();
//        IEnumerable<Element> getLocalElements();
//    }

//}