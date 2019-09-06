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
            if (node.Arguments.Count == 1 && node.Arguments[0].Kind == NodeKind.ObjectLiteralExpression)
            {
                ObjectLiteralExpression literaExpression = node.Arguments[0] as ObjectLiteralExpression;

                ObjectCreationExpressionSyntax csObjNewExpr = SyntaxFactory.ObjectCreationExpression(node.Type.ToCsNode<TypeSyntax>());
                InitializerExpressionSyntax csInitExpr = SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression);
                foreach (PropertyAssignment prop in literaExpression.Properties)
                {
                    csInitExpr = csInitExpr.AddExpressions(SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        prop.Name.ToCsNode<ExpressionSyntax>(),
                        prop.Initializer.ToCsNode<ExpressionSyntax>()));
                }
                return csObjNewExpr.WithInitializer(csInitExpr).AddArgumentListArguments();
            }
            else
            {
                return SyntaxFactory
                    .ObjectCreationExpression(node.Type.ToCsNode<TypeSyntax>())
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
        }

    }
}

