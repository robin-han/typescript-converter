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
    public class PostfixUnaryExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(PostfixUnaryExpression node)
        {
            SyntaxKind operatorKind = this.GetPostfixUnaryExpressionKind(node.Operator);
            if (operatorKind != SyntaxKind.None)
            {
                return SyntaxFactory.PostfixUnaryExpression(operatorKind, node.Operand.ToCsNode<ExpressionSyntax>());
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

