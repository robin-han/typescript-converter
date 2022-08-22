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
    public class ImportSpecifierConverter : NodeConverter
    {
        public JCTree Convert(ImportSpecifier specifier)
        {
            ImportDeclaration import = specifier.Ancestor<ImportDeclaration>();
            Document fromDoc = import?.FromDocument;
            Node definition = fromDoc?.GetExportTypeDeclaration(specifier.DefinitionName);
            if (definition != null)
            {
                string definitionPackage = definition.Document.GetPackageName();
                string propertyName = (string)definition.GetValue("NameText");

                // List<Node> genericParameters = definition.GetValue("TypeParameters") as List<Node>; // generic use using.
                JCIdent qualid = TreeMaker.Ident(Names.fromString($"{definitionPackage}.{propertyName}"));
                return TreeMaker.Import(qualid, false);
            }
            return null;
        }
    }
}

