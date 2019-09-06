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
    public class TypeOfExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(TypeOfExpression node)
        {
            //return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
            //    SyntaxKind.SimpleMemberAccessExpression,
            //    node.Expression.ToCsNode<ExpressionSyntax>(),
            //    SyntaxFactory.IdentifierName("GetTypeName")));

            return SyntaxFactory
               .InvocationExpression(SyntaxFactory.IdentifierName("TypeOf"))
               .AddArgumentListArguments(this.ToArgumentList(new List<Node>() { node.Expression }));
        }
    }
}

