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
    public class ArrayLiteralExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ArrayLiteralExpression node)
        {
            TypeSyntax csType = node.Type.ToCsNode<TypeSyntax>();
            if (node.Elements.Count == 0)
            {
                return SyntaxFactory
                    .ObjectCreationExpression(csType)
                    .AddArgumentListArguments();
            }
            else if (node.Elements.Count == 1 && node.Elements[0].Kind == NodeKind.SpreadElement)
            {
                Node argument = ((SpreadElement)node.Elements[0]).Expression;
                return SyntaxFactory
                   .ObjectCreationExpression(csType)
                   .AddArgumentListArguments(SyntaxFactory.Argument(argument.ToCsNode<ExpressionSyntax>()));
            }
            else
            {
                InitializerExpressionSyntax csInitilizerExprs = SyntaxFactory
                    .InitializerExpression(SyntaxKind.CollectionInitializerExpression)
                    .AddExpressions(node.Elements.ToCsNodes<ExpressionSyntax>());
                return SyntaxFactory
                    .ObjectCreationExpression(csType)
                    .WithInitializer(csInitilizerExprs);
            }
        }
    }

}

