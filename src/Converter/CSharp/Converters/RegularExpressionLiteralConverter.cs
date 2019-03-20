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
    public class RegularExpressionLiteralConverter : Converter
    {
        public CSharpSyntaxNode Convert(RegularExpressionLiteral node)
        {
            ExpressionSyntax csPattern = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(node.Pattern));

            ObjectCreationExpressionSyntax csObj = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName("Regex"))
                .AddArgumentListArguments(SyntaxFactory.Argument(csPattern));
            if (node.IgnoreCase)
            {
                ExpressionSyntax csRegOption = SyntaxFactory.MemberAccessExpression(
                      SyntaxKind.SimpleMemberAccessExpression,
                      SyntaxFactory.IdentifierName("RegexOptions"),
                      SyntaxFactory.IdentifierName("IgnoreCase"));
                csObj = csObj.AddArgumentListArguments(SyntaxFactory.Argument(csRegOption));
            }
            return csObj;
        }
    }
}

