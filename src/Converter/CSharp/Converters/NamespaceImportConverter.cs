using TypeScript.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class NamespaceImportConverter : Converter
    {
        public CSharpSyntaxNode Convert(NamespaceImport node)
        {
            ImportDeclaration import = node.Ancestor(NodeKind.ImportDeclaration) as ImportDeclaration;
            Document fromDoc = node.Project.GetDocumentByPath(import.ModulePath);

            return SyntaxFactory.UsingDirective(
                SyntaxFactory.NameEquals(node.Name.Text),
                SyntaxFactory.ParseName(fromDoc.GetPackageName()));
        }
    }
}
