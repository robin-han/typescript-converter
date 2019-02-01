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
    public class ObjectLiteralExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ObjectLiteralExpression node)
        {
            List<Node> objLiteralProps = node.Properties;

            Node type = node.Parent != null ? node.Parent.GetValue("Type") as Node : null;
            if (type != null && objLiteralProps.Count <= 1)
            {
                ObjectCreationExpressionSyntax dic = SyntaxFactory.ObjectCreationExpression(type.ToCsNode<TypeSyntax>()).AddArgumentListArguments();

                if (objLiteralProps.Count > 0)
                {
                    List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();
                    foreach (PropertyAssignment prop in objLiteralProps)
                    {
                        InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                            .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                            .AddExpressions(prop.Name.ToCsNode<ExpressionSyntax>(), prop.Initializer.ToCsNode<ExpressionSyntax>());

                        initItemExprs.Add(itemInitExpr);
                    }

                    dic = dic.WithInitializer(SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList(initItemExprs)));
                }
                return dic;
            }
            return SyntaxFactory.TupleExpression().AddArguments(objLiteralProps.ToCsNodes<ArgumentSyntax>());
        }
    }
}

