using TypeScript.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class ImportSpecifierConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ImportSpecifier specifier)
        {
            ImportDeclaration import = specifier.Ancestor<ImportDeclaration>();
            Document fromDoc = import?.FromDocument;
            Node definition = fromDoc?.GetExportTypeDeclaration(specifier.DefinitionName);
            if (definition != null)
            {
                string definitionPackage = definition.Document.GetPackageName();
                string package = import.Document.GetPackageName();
                string name = specifier.Name.Text;
                string propertyName = (string)definition.GetValue("NameText");

                List<Node> genericParameters = definition.GetValue("TypeParameters") as List<Node>;
                if (genericParameters != null && genericParameters.Count > 0 && package != definitionPackage) // generic use using.
                {
                    return SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(definitionPackage));
                }
                else if (package != definitionPackage || name != propertyName)
                {
                    return SyntaxFactory.UsingDirective(
                       SyntaxFactory.NameEquals(name),
                       SyntaxFactory.ParseName($"{definitionPackage}.{propertyName}"));
                }
            }
            return null;
        }
    }
}
