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
    public class SourceFileConverter : NodeConverter
    {
        public JCTree Convert(SourceFile sourceFile)
        {
            List<JCTree> defs = new List<JCTree>();

            //module declaration
            var mdDecls = sourceFile.ModuleDeclarations;
            if (mdDecls.Count > 0)
            {
                Node mdDecl = mdDecls[0];
                defs.AddRange(mdDecl.ToJavaSyntaxTrees<JCTree>());
            }

            //imports and exports
            List<Node> imports = sourceFile.ImportDeclarations;
            List<Node> typeAliases = sourceFile.TypeAliases;
            List<Node> typeDeclarations = this.FilterTypes(sourceFile.GetTypeDeclarations());
            if (imports.Count > 0 || typeAliases.Count > 0 || typeDeclarations.Count > 0)
            {
                string packageName = sourceFile.Document.GetPackageName();
                if (!string.IsNullOrEmpty(packageName))
                {
                    JCExpression packageExpr = TreeMaker.Ident(Names.fromString(packageName));
                    defs.Add(TreeMaker.PackageDecl(Nil<JCAnnotation>(), packageExpr));
                }
                defs.AddRange(imports.ToJavaSyntaxTrees<JCTree>());
                defs.AddRange(typeAliases.ToJavaSyntaxTrees<JCTree>());
                defs.AddRange(typeDeclarations.ToJavaSyntaxTrees<JCTree>());
            }

            return TreeMaker.TopLevel(defs);
        }
    }
}

