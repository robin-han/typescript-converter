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
    public class ObjectLiteralExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ObjectLiteralExpression node)
        {
            List<Node> properties = node.Properties;
            Node type = node.Type;

            if (type.Kind == NodeKind.TypeLiteral && !((TypeLiteral)type).IsIndexSignature && properties.Count >= 2)
            {
                return SyntaxFactory.TupleExpression().AddArguments(properties.ToCsSyntaxTrees<ArgumentSyntax>());
            }
            else if (type.Kind == NodeKind.AnyKeyword || type.Kind == NodeKind.VoidKeyword)
            {
                var csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                foreach (Node property in node.Properties)
                {
                    var (propName, valueExpr) = ConvertPropertyValue(property);
                    if (propName == null) continue;

                    csAnonyNewExpr = csAnonyNewExpr.AddInitializers(SyntaxFactory.AnonymousObjectMemberDeclarator(
                        SyntaxFactory.NameEquals(NormalizeTypeName(propName)),
                        valueExpr));
                }
                return csAnonyNewExpr;
            }
            else
            {
                var csType = type.ToCsSyntaxTree<TypeSyntax>();
                var csObjLiteral = SyntaxFactory.ObjectCreationExpression(csType)
                    .AddArgumentListArguments();
                var initItemExprs = new List<ExpressionSyntax>();
                foreach (Node property in node.Properties)
                {
                    var (propName, valueExpr) = ConvertPropertyValue(property);
                    if (propName == null) continue;

                    var csNameExpression = SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(NormalizeTypeName(propName)));
                    var itemInitExpr = SyntaxFactory
                        .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                        .AddExpressions(csNameExpression, valueExpr);

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

        private (Node, ExpressionSyntax) ConvertPropertyValue(Node property)
        {
            Node propName = null;
            ExpressionSyntax valueExpr = null;

            switch (property.Kind)
            {
                case NodeKind.PropertyAssignment:
                    var prop = (PropertyAssignment)property;
                    propName = prop.Name;
                    valueExpr = prop.Initializer.ToCsSyntaxTree<ExpressionSyntax>();
                    break;

                case NodeKind.ShorthandPropertyAssignment:
                    var shortProp = (ShorthandPropertyAssignment)property;
                    propName = shortProp.Name;
                    valueExpr = SyntaxFactory.ParseName(NormalizeTypeName(propName));
                    break;

                case NodeKind.SpreadAssignment:
                    //TODO: spread
                    return (null, null);

                default:
                    return (null, null);
            }

            return (propName, valueExpr);
        }
    }
}
