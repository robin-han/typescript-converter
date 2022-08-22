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
    public class ImportClauseConverter : NodeConverter
    {
        public List<JCTree> Convert(ImportClause node)
        {
            // import A, {B, C} from 'xxxxxx';
            List<JCTree> imports = new List<JCTree>();

            if (node.Name != null) // default
            {
                ImportDeclaration import = node.Ancestor<ImportDeclaration>();
                Document fromDoc = import?.FromDocument;
                Node definition = fromDoc?.GetExportDefaultTypeDeclaration();
                if (definition != null)
                {
                    string definitionPackage = definition.Document.GetPackageName();
                    string propertyName = (string)definition.GetValue("NameText");

                    JCIdent qualid = TreeMaker.Ident(Names.fromString($"{definitionPackage}.{propertyName}"));
                    imports.Add(TreeMaker.Import(qualid, false));
                }

                //
                if (node.NamedBindings != null)
                {
                    imports.AddRange(node.NamedBindings.ToJavaSyntaxTrees<JCTree>());
                }
            }

            return imports;
        }
    }
}

