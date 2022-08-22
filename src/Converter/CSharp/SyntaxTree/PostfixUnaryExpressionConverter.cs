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
    public class PostfixUnaryExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(PostfixUnaryExpression node)
        {
            SyntaxKind operatorKind = this.GetPostfixUnaryExpressionKind(node.Operator);
            if (operatorKind != SyntaxKind.None)
            {
                return SyntaxFactory.PostfixUnaryExpression(operatorKind, node.Operand.ToCsSyntaxTree<ExpressionSyntax>());
            }
            return null;
        }

        private SyntaxKind GetPostfixUnaryExpressionKind(NodeKind @operator)
        {
            switch (@operator)
            {
                case NodeKind.PlusPlusToken:
                    return SyntaxKind.PostIncrementExpression;
                case NodeKind.MinusMinusToken:
                    return SyntaxKind.PostDecrementExpression;
                default:
                    return SyntaxKind.None;
            }
        }
    }
}

