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
    public class ConstructorConverter : Converter
    {
        public CSharpSyntaxNode Convert(Constructor node)
        {
            if (!(node.Parent is ClassDeclaration tsClassNode))
            {
                return null;
            }

            ConstructorDeclarationSyntax csCtor = SyntaxFactory.ConstructorDeclaration(tsClassNode.Name.Text);

            csCtor = csCtor.AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());
            csCtor = csCtor.AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            ExpressionStatement baseNode = node.BaseConstructor as ExpressionStatement;
            if (baseNode != null)
            {
                ArgumentSyntax[] csArgs = this.ToArgumentList((baseNode.Expression as CallExpression).Arguments);
                csCtor = csCtor.WithInitializer(SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer).AddArgumentListArguments(csArgs));
            }
            if (node.JsDoc.Count > 0)
            {
                csCtor = csCtor.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            return csCtor.WithBody(node.Body.ToCsNode<BlockSyntax>());
        }
    }
}

