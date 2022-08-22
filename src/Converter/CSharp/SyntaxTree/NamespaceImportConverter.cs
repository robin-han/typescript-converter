using TypeScript.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class NamespaceImportConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(NamespaceImport node)
        {
            ImportDeclaration import = node.Ancestor<ImportDeclaration>();
            Document fromDoc = node.Document.Project.GetDocument(import.ModulePath);
            if (fromDoc != null)
            {
                return SyntaxFactory.UsingDirective(
                    SyntaxFactory.NameEquals(node.Name.Text),
                    SyntaxFactory.ParseName(fromDoc.GetPackageName()));
            }
            return null;
        }
    }
}
