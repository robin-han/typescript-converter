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
    public class GetAccessorConverter : Converter
    {
        public CSharpSyntaxNode Convert(GetAccessor node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            AccessorDeclarationSyntax csGetAccess = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration);
            if (node.Body != null)
            {
                csGetAccess = csGetAccess.WithBody(node.Body.ToCsNode<BlockSyntax>());
            }
            else
            {
                csGetAccess = csGetAccess.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }

            return csProperty.AddAccessorListAccessors(csGetAccess);
        }
    }
}

