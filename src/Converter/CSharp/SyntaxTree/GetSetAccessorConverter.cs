using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class GetSetAccessorConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(GetSetAccessor node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory.PropertyDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text);
            csProperty = csProperty.AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>());

            csProperty = csProperty.AddAttributeLists(node.GetAccessor.Decorators.ToCsSyntaxTrees<AttributeListSyntax>());
            csProperty = csProperty.AddAttributeLists(node.SetAccessor.Decorators.ToCsSyntaxTrees<AttributeListSyntax>());

            AccessorDeclarationSyntax csGetAccess = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithBody(node.GetAccessor.Body.ToCsSyntaxTree<BlockSyntax>());

            AccessorDeclarationSyntax csSetAccess = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithBody(node.SetAccessor.Body.ToCsSyntaxTree<BlockSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            return csProperty.AddAccessorListAccessors(csGetAccess, csSetAccess);
        }
    }
}
