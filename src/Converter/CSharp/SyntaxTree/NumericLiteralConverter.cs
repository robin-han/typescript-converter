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
    public class NumericLiteralConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(NumericLiteral node)
        {
            string text = node.Text;
            if (Zeroize(node))
            {
                text += ".0";
            }
            return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(text, double.Parse(text)));
        }

        private bool Zeroize(NumericLiteral node)
        {
            BinaryExpression expr = node.Parent == null ? null : node.Parent as BinaryExpression;
            if (expr != null &&
                expr.OperatorToken.Kind == NodeKind.SlashToken &&
                expr.Left.Kind == NodeKind.NumericLiteral &&
                expr.Right.Kind == NodeKind.NumericLiteral &&
                node.Text.IndexOf(".") == -1)
            {
                return true;
            }
            return false;
        }
    }
}

