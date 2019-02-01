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
    public class TypeAssertionExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(TypeAssertionExpression node)
        {
            // var a = <HTMLElement>div;
            return SyntaxFactory.CastExpression(
                node.Type.ToCsNode<TypeSyntax>(),
                node.Expression.ToCsNode<ExpressionSyntax>());
        }
    }
}

