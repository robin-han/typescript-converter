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
    public class MethodDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(MethodDeclaration node)
        {
            MethodDeclarationSyntax csMethod = SyntaxFactory
                .MethodDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>())
                .AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csMethod = csMethod.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                csMethod = csMethod.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            if (node.Body == null)
            {
                csMethod = csMethod.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                csMethod = csMethod.WithBody(node.Body.ToCsNode<BlockSyntax>());
            }

            return csMethod;
        }

    }
}

