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
        public CSharpSyntaxNode Convert(ImportSpecifier specifier)
        {
            ImportDeclaration import = specifier.Ancestor(NodeKind.ImportDeclaration) as ImportDeclaration;
            Document fromDoc = import?.FromDocument;
            Node definition = fromDoc?.GetExportTypeDefinition(specifier.DefinitionName);
            if (definition != null)
            {
                List<Node> genericParameters = definition.GetValue("TypeParameters") as List<Node>;
                if (genericParameters != null && genericParameters.Count > 0) // generic use using.
                {
                    return SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(definition.Document.GetPackageName()));
                }
                else
                {
                    return SyntaxFactory.UsingDirective(
                       SyntaxFactory.NameEquals(specifier.Name.Text),
                       SyntaxFactory.ParseName(definition.Document.GetPackageName() + "." + definition.GetValue("NameText")));
                }
            }
            return null;
        }
    }
}
