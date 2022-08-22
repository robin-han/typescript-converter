using System.Collections.Generic;

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
    /// Represents the abstract syntax tree for compilation units (source
    /// files) and package declarations (package-info.java).
    /// 
    /// @jls 7.3 Compilation Units
    /// @jls 7.4 Package Declarations
    /// 
    /// @author Peter von der Ah&eacute;
    /// @since 1.6
    /// </summary>
    public interface CompilationUnitTree : Tree
    {
        /// <summary>
        /// Returns the annotations listed on any package declaration
        /// at the head of this compilation unit, or {@code null} if there
        /// is no package declaration. </summary>
        /// <returns> the package annotations </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends AnnotationTree> getPackageAnnotations();
        IList<AnnotationTree> getPackageAnnotations();

        /// <summary>
        /// Returns the name contained in any package declaration
        /// at the head of this compilation unit, or {@code null} if there
        /// is no package declaration. </summary>
        /// <returns> the package name </returns>
        ExpressionTree getPackageName();

        /// <summary>
        /// Returns the package tree associated with this compilation unit,
        /// or {@code null} if there is no package declaration. </summary>
        /// <returns> the package tree
        /// @since 9 </returns>
        PackageTree getPackage();

        /// <summary>
        /// Returns the import declarations appearing in this compilation unit. </summary>
        /// <returns> the import declarations </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends ImportTree> getImports();
        IList<ImportTree> getImports();

        /// <summary>
        /// Returns the type declarations appearing in this compilation unit.
        /// The list may also include empty statements resulting from
        /// extraneous semicolons. </summary>
        /// <returns> the type declarations </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends Tree> getTypeDecls();
        IList<Tree> getTypeDecls();

        ///// <summary>
        ///// Returns the file object containing the source for this compilation unit. </summary>
        ///// <returns> the file object </returns>
        //JavaFileObject getSourceFile();

        /// <summary>
        /// Returns the line map for this compilation unit, if available.
        /// Returns {@code null} if the line map is not available. </summary>
        /// <returns> the line map for this compilation unit </returns>
        LineMap getLineMap();
    }

}