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
                    string definitionPackage = definition.Document.GetPackageName();
                    string package = import.Document.GetPackageName();
                    string name = node.Name.Text;
                    string propertyName = (string)definition.GetValue("NameText");

                    if (package != definitionPackage || name != propertyName)
                    {
                        UsingDirectiveSyntax usingSyntax = SyntaxFactory.UsingDirective(
                            SyntaxFactory.NameEquals(name),
                            SyntaxFactory.ParseName($"{definitionPackage}.{propertyName}"));
                        usings = usings.Add(usingSyntax);
                    }
                }
            }

            if (node.NamedBindings != null)
            {
                switch (node.NamedBindings.Kind)
                {
                    case NodeKind.NamespaceImport:
                        UsingDirectiveSyntax usingSyntax = node.NamedBindings.ToCsNode<UsingDirectiveSyntax>();
                        if (usingSyntax != null)
                        {
                            usings = usings.Add(usingSyntax);
                        }
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
