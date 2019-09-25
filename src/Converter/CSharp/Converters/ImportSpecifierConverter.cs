using TypeScript.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class ImportSpecifierConverter : Converter
    {
        public CSharpSyntaxNode Convert(ImportSpecifier node)
        {
            ImportDeclaration import = node.Ancestor(NodeKind.ImportDeclaration) as ImportDeclaration;
            Document fromDoc = node.Project.GetDocumentByPath(import.ModulePath);
            if (fromDoc != null)
            {
                string typeName = node.PropertyName != null ? fromDoc.GetExportActualName(node.PropertyName.Text) : fromDoc.GetExportActualName(node.Name.Text);
                return SyntaxFactory.UsingDirective(
                   SyntaxFactory.NameEquals(node.Name.Text),
                   SyntaxFactory.ParseName(fromDoc.GetPackageName() + "." + typeName));
            }
            return null;
        }
    }
}
