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
    public class GetSetAccessorConverter : Converter
    {
        public CSharpSyntaxNode Convert(GetSetAccessor node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory.PropertyDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text);
            csProperty = csProperty.AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());

            AccessorDeclarationSyntax csGetAccess = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithBody(node.GetAccessor.Body.ToCsNode<BlockSyntax>());

            AccessorDeclarationSyntax csSetAccess = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithBody(node.SetAccessor.Body.ToCsNode<BlockSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            return csProperty.AddAccessorListAccessors(csGetAccess, csSetAccess);
        }
    }
}
