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
    public class PropertyAccessExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(PropertyAccessExpression node)
        {
            SimpleNameSyntax csName = null;

            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                csName = SyntaxFactory
                    .GenericName(node.Name.Text)
                    .AddTypeArgumentListArguments(typeArguments.ToCsNodes<TypeSyntax>());
            }
            else
            {
                csName = node.Name.ToCsNode<SimpleNameSyntax>();
            }

            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node.Expression.ToCsNode<ExpressionSyntax>(), csName);
        }
    }
}

