using TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class ImportClauseConverter : Converter
    {
        public SyntaxList<UsingDirectiveSyntax> Convert(ImportClause node)
        {
            SyntaxList<UsingDirectiveSyntax> usings = new SyntaxList<UsingDirectiveSyntax>();
            if (node.Name != null) // default
            {
                ImportDeclaration import = node.Ancestor(NodeKind.ImportDeclaration) as ImportDeclaration;
                Syntax.Document fromDoc = import?.FromDocument;
                Node definition = fromDoc?.GetExportDefaultTypeDefinition();
                if (definition != null)
                {
                    UsingDirectiveSyntax usignSyntax = SyntaxFactory.UsingDirective(
                        SyntaxFactory.NameEquals(node.Name.Text),
                        SyntaxFactory.ParseName(definition.Document.GetPackageName() + "." + definition.GetValue("NameText")));
                    usings = usings.Add(usignSyntax);
                }
            }

            if (node.NamedBindings != null)
            {
                switch (node.NamedBindings.Kind)
                {
                    case NodeKind.NamespaceImport:
                        usings = usings.Add(node.NamedBindings.ToCsNode<UsingDirectiveSyntax>());
                        break;

                    case NodeKind.NamedImports:
                        usings = usings.AddRange(node.NamedBindings.ToCsNode<IEnumerable<UsingDirectiveSyntax>>());
                        break;

                    default:
                        break;
                }
            }

            return usings;
        }
    }
}
