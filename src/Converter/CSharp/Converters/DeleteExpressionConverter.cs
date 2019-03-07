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
    public class DeleteExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(DeleteExpression node)
        {
            if (node.Expression.Kind == NodeKind.ElementAccessExpression)
            {
                ElementAccessExpression elementAccessNode = node.Expression as ElementAccessExpression;

                MemberAccessExpressionSyntax csPropertyAccess = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    elementAccessNode.Expression.ToCsNode<ExpressionSyntax>(),
                    SyntaxFactory.IdentifierName("remove"));

                return SyntaxFactory
                    .InvocationExpression(csPropertyAccess)
                    .AddArgumentListArguments(this.ToArgumentList(new List<Node>() { elementAccessNode.ArgumentExpression }));
            }

            //TODO: NOT SUPPORT
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }
    }
}

