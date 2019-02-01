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
    public class PrefixUnaryExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(PrefixUnaryExpression node)
        {
            SyntaxKind operatorKind = this.GetPrefixUnaryExpressionKind(node.Operator);
            if (operatorKind != SyntaxKind.None)
            {
                return SyntaxFactory.PrefixUnaryExpression(operatorKind, node.Operand.ToCsNode<ExpressionSyntax>());
            }
            return null;
        }

        private SyntaxKind GetPrefixUnaryExpressionKind(NodeKind @operator)
        {
            switch (@operator)
            {
                case NodeKind.PlusToken:
                    return SyntaxKind.UnaryPlusExpression;

                case NodeKind.MinusToken:
                    return SyntaxKind.UnaryMinusExpression;

                case NodeKind.TildeToken:
                    return SyntaxKind.BitwiseNotExpression;

                case NodeKind.ExclamationToken:
                    return SyntaxKind.LogicalNotExpression;

                case NodeKind.PlusPlusToken:
                    return SyntaxKind.PreIncrementExpression;

                case NodeKind.MinusMinusToken:
                    return SyntaxKind.PreDecrementExpression;

                case NodeKind.AsteriskToken:
                    return SyntaxKind.PointerIndirectionExpression;

                case NodeKind.AmpersandToken:
                    return SyntaxKind.AddressOfExpression;

                default:
                    return SyntaxKind.None;
            }
        }
    }
}

