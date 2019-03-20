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
            List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();

            foreach (PropertyAssignment prop in properties)
            {
                ExpressionSyntax csNameExpression = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(prop.Name.Text));
                InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                    .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                    .AddExpressions(csNameExpression, prop.Initializer.ToCsNode<ExpressionSyntax>());

                initItemExprs.Add(itemInitExpr);
            }

            Node type = node.Type;
            if (type.Kind == NodeKind.TypeLiteral && properties.Count >= 2)
            {
                return SyntaxFactory.TupleExpression().AddArguments(properties.ToCsNodes<ArgumentSyntax>());
            }

            ObjectCreationExpressionSyntax csObjLiteral = SyntaxFactory.ObjectCreationExpression(type.ToCsNode<TypeSyntax>()).AddArgumentListArguments();
            if (initItemExprs.Count > 0)
            {
                csObjLiteral = csObjLiteral.WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.CollectionInitializerExpression, SyntaxFactory.SeparatedList(initItemExprs)));
            }
            return csObjLiteral;
        }

    }
}

