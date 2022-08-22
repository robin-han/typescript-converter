using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;

namespace TypeScript.Converter.Java
{
    public class NamespaceImportConverter : NodeConverter
    {
        public JCTree Convert(NamespaceImport node)
        {
            ImportDeclaration import = node.Ancestor<ImportDeclaration>();
            Document fromDoc = node.Document.Project.GetDocument(import.ModulePath);
            if (fromDoc != null)
            {
                JCIdent qualid = TreeMaker.Ident(Names.fromString($"{fromDoc.GetPackageName()}.*"));
                return TreeMaker.Import(qualid, false);
            }
            return null;
        }
    }
}
