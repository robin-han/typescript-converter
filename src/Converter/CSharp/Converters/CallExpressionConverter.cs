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
            if (this.IsTypePredicate(node, out FunctionDeclaration predicateFunc))
            {
                return SyntaxFactory.BinaryExpression(
                    SyntaxKind.IsExpression,
                    node.Arguments[0].ToCsNode<ExpressionSyntax>(),
                    ((TypePredicate)predicateFunc.Type).Type.ToCsNode<ExpressionSyntax>());
            }
            else if (node.Expression.Text == "Number")
            {
                return SyntaxFactory
                    .InvocationExpression(SyntaxFactory.ParseExpression("ToNumber"))
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
            else if (node.Expression.Text == "String")
            {
                return SyntaxFactory
                    .InvocationExpression(SyntaxFactory.ParseExpression("ToString"))
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
            else
            {
                return SyntaxFactory
                    .InvocationExpression(node.Expression.ToCsNode<ExpressionSyntax>())
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
        }

        private bool IsTypePredicate(CallExpression node, out FunctionDeclaration typePredicateFunc)
        {
            typePredicateFunc = null;
            string text = node.Expression.Text;
            int index = text.LastIndexOf('.');
            if (index >= 0)
            {
                text = text.Substring(index + 1);
            }

            if (text.StartsWith("_is") && node.Arguments.Count == 1)
            {
                typePredicateFunc = node.Project.GlobalFunctions.Find(n =>
                {
                    FunctionDeclaration func = (FunctionDeclaration)n;
                    return (func.Name.Text == text && func.Type != null && func.Type.Kind == NodeKind.TypePredicate);
                }) as FunctionDeclaration;
            }
            return typePredicateFunc != null;
        }

    }
}

