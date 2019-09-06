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
            List<Node> properties = node.Properties;
            Node type = node.Type;
            if (type.Kind == NodeKind.TypeLiteral && properties.Count >= 2)
            {
                return SyntaxFactory.TupleExpression().AddArguments(properties.ToCsNodes<ArgumentSyntax>());
            }
            else if (type.Kind == NodeKind.AnyKeyword)
            {
                AnonymousObjectCreationExpressionSyntax csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                foreach (PropertyAssignment prop in node.Properties)
                {
                    string propName = prop.Name.Text;
                    Node initValue = prop.Initializer;
                    ExpressionSyntax valueExpr = initValue.ToCsNode<ExpressionSyntax>();

                    if (type.Kind == NodeKind.TypeLiteral && initValue.Kind == NodeKind.NullKeyword)
                    {
                        Node memType = TypeHelper.GetTypeLiteralMemberType(type as TypeLiteral, propName);
                        if (memType != null)
                        {
                            valueExpr = SyntaxFactory.CastExpression(memType.ToCsNode<TypeSyntax>(), valueExpr);
                        }
                    }

                    csAnonyNewExpr = csAnonyNewExpr.AddInitializers(SyntaxFactory.AnonymousObjectMemberDeclarator(
                        SyntaxFactory.NameEquals(propName),
                        valueExpr));
                }
                return csAnonyNewExpr;
            }
            else
            {
                ObjectCreationExpressionSyntax csObjLiteral = SyntaxFactory.ObjectCreationExpression(type.ToCsNode<TypeSyntax>()).AddArgumentListArguments();
                List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();
                foreach (PropertyAssignment prop in properties)
                {
                    ExpressionSyntax csNameExpression = SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(prop.Name.Text));
                    InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                        .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                        .AddExpressions(csNameExpression, prop.Initializer.ToCsNode<ExpressionSyntax>());

                    initItemExprs.Add(itemInitExpr);
                }
                if (initItemExprs.Count > 0)
                {
                    return csObjLiteral.WithInitializer(SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList(initItemExprs)));
                }
                return csObjLiteral;
            }
        }

    }
}

