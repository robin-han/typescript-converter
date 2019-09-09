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
    public class MethodSignatureConverter : Converter
    {
        public CSharpSyntaxNode Convert(MethodSignature node)
        {
            MethodDeclarationSyntax csMethod = SyntaxFactory
                .MethodDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csMethod = csMethod.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            if (node.TypeParameters.Count > 0)
            {
                csMethod = csMethod.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            return csMethod.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
    }

}

