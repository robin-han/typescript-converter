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

            CallExpression baseNode =
                node.Base is ExpressionStatement
                ? ((ExpressionStatement)node.Base).Expression as CallExpression
                : node.Base as CallExpression;

            if (baseNode != null)
            {
                ArgumentSyntax[] csArgs = this.ToArgumentList(baseNode.Arguments);
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

