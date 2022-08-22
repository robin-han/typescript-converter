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
    public class TypeAssertionExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TypeAssertionExpression node)
        {
            // var a = <HTMLElement>div;
            return SyntaxFactory.CastExpression(
                node.Type.ToCsSyntaxTree<TypeSyntax>(),
                node.Expression.ToCsSyntaxTree<ExpressionSyntax>());
        }
    }
}

