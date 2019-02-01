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
    public class ArrayLiteralExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ArrayLiteralExpression node)
        {
            InitializerExpressionSyntax csInitilizerExprs = SyntaxFactory
                .InitializerExpression(SyntaxKind.CollectionInitializerExpression)
                .AddExpressions(node.Elements.ToCsNodes<ExpressionSyntax>());

            return SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory
                    .GenericName("Array")
                    .AddTypeArgumentListArguments(node.Type.ToCsNode<TypeSyntax>()))
                .WithInitializer(csInitilizerExprs);
        }
    }

}

