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
    public class CallExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(CallExpression node)
        {
            return SyntaxFactory
                .InvocationExpression(node.Expression.ToCsNode<ExpressionSyntax>())
                .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
        }

    }
}

