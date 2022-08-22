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
    public class CallExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(CallExpression node)
        {
            if (node.IsTypePredicate(out FunctionDeclaration predicateFunc))
            {
                return SyntaxFactory.BinaryExpression(
                    SyntaxKind.IsExpression,
                    node.Arguments[0].ToCsSyntaxTree<ExpressionSyntax>(),
                    ((TypePredicate)predicateFunc.Type).Type.ToCsSyntaxTree<ExpressionSyntax>());
            }
            else if (IsNumberInvoke(node))
            {
                return SyntaxFactory
                    .InvocationExpression(SyntaxFactory.ParseExpression("ToNumber"))
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
            else if (IsStringInvoke(node))
            {
                return SyntaxFactory
                    .InvocationExpression(SyntaxFactory.ParseExpression("ToString"))
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
            else
            {
                // Judge.asEnum(geometry.type, GeoType, true);
                List<Node> arguments = new List<Node>(node.Arguments);
                if (IsAsEnumInvoke(node))
                {
                    arguments.RemoveAt(1);
                }
                return SyntaxFactory
                    .InvocationExpression(node.Expression.ToCsSyntaxTree<ExpressionSyntax>())
                    .AddArgumentListArguments(this.ToArgumentList(arguments));
            }
        }

        /// <summary>
        /// Number(xxx)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsNumberInvoke(CallExpression node)
        {
            return node.Expression.Text == "Number";
        }

        /// <summary>
        /// String();
        /// </summary>
        /// <returns></returns>
        private bool IsStringInvoke(CallExpression node)
        {
            return node.Expression.Text == "String";
        }

        internal static bool IsAsEnumInvoke(CallExpression node)
        {
            return (node.Arguments.Count >= 2) && (node.Expression is PropertyAccessExpression propAccess) && (propAccess.Name.Text == "asEnum");
        }
    }
}

