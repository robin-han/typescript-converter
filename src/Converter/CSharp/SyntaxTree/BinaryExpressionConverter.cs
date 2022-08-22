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
    public class BinaryExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(BinaryExpression node)
        {
            NodeKind operatorKind = node.OperatorToken.Kind;

            //assign
            SyntaxKind assignKind = GetAssignmentExpressionKind(operatorKind);
            if (assignKind != SyntaxKind.None)
            {
                return SyntaxFactory.AssignmentExpression(
                    assignKind,
                    node.Left.ToCsSyntaxTree<ExpressionSyntax>(),
                    node.Right.ToCsSyntaxTree<ExpressionSyntax>());
            }

            //
            SyntaxKind binaryKind = GetBinaryExpressionKind(operatorKind);
            if (binaryKind != SyntaxKind.None)
            {
                return SyntaxFactory.BinaryExpression(
                    binaryKind,
                    node.Left.ToCsSyntaxTree<ExpressionSyntax>(),
                    node.Right.ToCsSyntaxTree<ExpressionSyntax>());
            }
            
            //TODO: NOT SUPPORT
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }

        private SyntaxKind GetAssignmentExpressionKind(NodeKind nodeKind)
        {
            switch (nodeKind)
            {
                case NodeKind.EqualsToken:
                    return SyntaxKind.SimpleAssignmentExpression;

                case NodeKind.PlusEqualsToken:
                    return SyntaxKind.AddAssignmentExpression;

                case NodeKind.MinusEqualsToken:
                    return SyntaxKind.SubtractAssignmentExpression;

                case NodeKind.AsteriskEqualsToken:
                    return SyntaxKind.MultiplyAssignmentExpression;

                case NodeKind.SlashEqualsToken:
                    return SyntaxKind.DivideAssignmentExpression;

                case NodeKind.PercentEqualsToken:
                    return SyntaxKind.ModuloAssignmentExpression;

                case NodeKind.LessThanLessThanEqualsToken:
                    return SyntaxKind.LeftShiftAssignmentExpression;

                case NodeKind.GreaterThanGreaterThanEqualsToken:
                    return SyntaxKind.RightShiftAssignmentExpression;

                case NodeKind.AmpersandEqualsToken:
                    return SyntaxKind.AndAssignmentExpression;

                case NodeKind.BarEqualsToken:
                    return SyntaxKind.OrAssignmentExpression;

                case NodeKind.CaretEqualsToken:
                    return SyntaxKind.ExclusiveOrAssignmentExpression;

                default:
                    return SyntaxKind.None;
            }
        }

        private SyntaxKind GetBinaryExpressionKind(NodeKind nodeKind)
        {
            switch (nodeKind)
            {
                case NodeKind.PlusToken:
                    return SyntaxKind.AddExpression;

                case NodeKind.MinusToken:
                    return SyntaxKind.SubtractExpression;

                case NodeKind.AsteriskToken:
                    return SyntaxKind.MultiplyExpression;

                case NodeKind.SlashToken:
                    return SyntaxKind.DivideExpression;

                case NodeKind.PercentToken:
                    return SyntaxKind.ModuloExpression;

                case NodeKind.LessThanLessThanToken:
                    return SyntaxKind.LeftShiftExpression;

                case NodeKind.GreaterThanGreaterThanToken:
                case NodeKind.GreaterThanGreaterThanGreaterThanToken:
                    return SyntaxKind.RightShiftExpression;

                case NodeKind.BarBarToken:
                    return SyntaxKind.LogicalOrExpression;

                case NodeKind.AmpersandAmpersandToken:
                    return SyntaxKind.LogicalAndExpression;

                case NodeKind.BarToken:
                    return SyntaxKind.BitwiseOrExpression;

                case NodeKind.AmpersandToken:
                    return SyntaxKind.BitwiseAndExpression;

                case NodeKind.CaretToken:
                    return SyntaxKind.ExclusiveOrExpression;

                case NodeKind.EqualsEqualsToken:
                case NodeKind.EqualsEqualsEqualsToken:
                    return SyntaxKind.EqualsExpression;

                case NodeKind.ExclamationEqualsToken:
                case NodeKind.ExclamationEqualsEqualsToken:
                    return SyntaxKind.NotEqualsExpression;

                case NodeKind.LessThanToken:
                    return SyntaxKind.LessThanExpression;

                case NodeKind.LessThanEqualsToken:
                    return SyntaxKind.LessThanOrEqualExpression;

                case NodeKind.GreaterThanToken:
                    return SyntaxKind.GreaterThanExpression;

                case NodeKind.GreaterThanEqualsToken:
                    return SyntaxKind.GreaterThanOrEqualExpression;

                case NodeKind.InstanceOfKeyword:
                case NodeKind.IsKeyword:
                    return SyntaxKind.IsExpression;

                case NodeKind.AsKeyword:
                    return SyntaxKind.AsExpression;

                //case NodeKind.QuestionQuestionToken:
                //    return SyntaxKind.CoalesceExpression;

                default:
                    return SyntaxKind.None;
            }

            //InKeyword in
            //LessThanSlashToken </
            //EqualsGreaterThanToken =>
            //AsteriskAsteriskToken **
            //PlusPlusToken ++
            //MinusMinusToken --
            //GreaterThanGreaterThanGreaterThanToken >>>
            //ExclamationToken ^
            //TildeToken ~
            //QuestionToken ?
            //ColonToken :
            //AtToken @
        }

    }
}

