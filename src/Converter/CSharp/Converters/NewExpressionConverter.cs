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
    public class NewExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(NewExpression node)
        {
            ObjectCreationExpressionSyntax csNewExpr = SyntaxFactory.ObjectCreationExpression(node.Type.ToCsNode<TypeSyntax>());
            if (node.Arguments.Count == 1 && node.Arguments[0].Kind == NodeKind.ObjectLiteralExpression)
            {
                InitializerExpressionSyntax csInitExpr = SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression);
                ObjectLiteralExpression objLiteraNode = node.Arguments[0] as ObjectLiteralExpression;
                foreach (PropertyAssignment prop in objLiteraNode.Properties)
                {
                    csInitExpr = csInitExpr.AddExpressions(SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        prop.Name.ToCsNode<ExpressionSyntax>(),
                        prop.Initializer.ToCsNode<ExpressionSyntax>()));
                }
                return csNewExpr.WithInitializer(csInitExpr).AddArgumentListArguments();
            }
            else
            {
                return csNewExpr.AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
        }

    }
}

