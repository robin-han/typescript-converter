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
    public class CallExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(CallExpression node)
        {
            if (node.Expression.Text == "Number")
            {
                return SyntaxFactory
                  .InvocationExpression(SyntaxFactory.ParseExpression("ToNumber"))
                  .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }

            return SyntaxFactory
                .InvocationExpression(node.Expression.ToCsNode<ExpressionSyntax>())
                .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
        }

    }
}

