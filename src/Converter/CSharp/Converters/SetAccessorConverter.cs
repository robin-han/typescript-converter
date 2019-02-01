using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class SetAccessorConverter : Converter
    {
        public CSharpSyntaxNode Convert(SetAccessor node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Parameters[0].Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            AccessorDeclarationSyntax csSetAccess = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration);
            if (node.Body != null)
            {
                csSetAccess = csSetAccess.WithBody(node.Body.ToCsNode<BlockSyntax>());
            }

            return csProperty.AddAccessorListAccessors(csSetAccess);
        }
    }
}

