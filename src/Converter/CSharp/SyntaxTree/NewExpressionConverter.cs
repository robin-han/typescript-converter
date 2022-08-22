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
    public class NewExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(NewExpression node)
        {
            if (node.Arguments.Count == 1 && node.Arguments[0].Kind == NodeKind.ObjectLiteralExpression)
            {
                ObjectLiteralExpression literaExpression = node.Arguments[0] as ObjectLiteralExpression;

                ObjectCreationExpressionSyntax csObjNewExpr = SyntaxFactory.ObjectCreationExpression(node.Type.ToCsSyntaxTree<TypeSyntax>());
                InitializerExpressionSyntax csInitExpr = SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression);
                foreach (PropertyAssignment prop in literaExpression.Properties)
                {
                    csInitExpr = csInitExpr.AddExpressions(SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        prop.Name.ToCsSyntaxTree<ExpressionSyntax>(),
                        prop.Initializer.ToCsSyntaxTree<ExpressionSyntax>()));
                }
                return csObjNewExpr.WithInitializer(csInitExpr).AddArgumentListArguments();
            }


            if (node.TypeArguments.Count > 0)
            {
                return SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory
                    .GenericName(this.TrimTypeName(node.Type.Text))
                    .AddTypeArgumentListArguments(node.TypeArguments.ToCsSyntaxTrees<TypeSyntax>()))
                .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
            else
            {
                return SyntaxFactory
                    .ObjectCreationExpression(node.Type.ToCsSyntaxTree<TypeSyntax>())
                    .AddArgumentListArguments(this.ToArgumentList(node.Arguments));
            }
        }

    }
}

