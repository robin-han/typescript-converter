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
    public class RegularExpressionLiteralConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(RegularExpressionLiteral node)
        {
            ExpressionSyntax patternExpr = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(node.Pattern));
            ObjectCreationExpressionSyntax csObj = SyntaxFactory.ObjectCreationExpression(
               SyntaxFactory.IdentifierName("RegExp"))
               .AddArgumentListArguments(SyntaxFactory.Argument(patternExpr));

            if (!string.IsNullOrEmpty(node.SearchFlags))
            {
                ExpressionSyntax flagsExpr = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(node.SearchFlags));
                csObj = csObj.AddArgumentListArguments(SyntaxFactory.Argument(flagsExpr));
            }
            return csObj;
        }
    }
}

