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
    public class AsExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(AsExpression node)
        {
            if (TypeHelper.IsArrayType(node.Type)) //to .AsArray<T>()
            {
                GenericNameSyntax csName = SyntaxFactory.GenericName("AsArray");

                if (node.Type.Kind == NodeKind.ArrayType)
                {
                    csName = csName.AddTypeArgumentListArguments((node.Type as ArrayType).ElementType.ToCsNode<TypeSyntax>());
                }
                else if (node.Type.Kind == NodeKind.TypeReference)
                {
                    csName = csName.AddTypeArgumentListArguments((node.Type as TypeReference).TypeArguments[0].ToCsNode<TypeSyntax>());
                }

                return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        node.Expression.ToCsNode<ExpressionSyntax>(),
                        csName))
                    .AddArgumentListArguments();
            }
            else if (TypeHelper.GetName(node.Type.Text) == "DataValueType")
            {
                GenericNameSyntax csName = SyntaxFactory.GenericName("As");
                csName = csName.AddTypeArgumentListArguments(node.Type.ToCsNode<TypeSyntax>());

                return SyntaxFactory.InvocationExpression(
                   SyntaxFactory.MemberAccessExpression(
                       SyntaxKind.SimpleMemberAccessExpression,
                       node.Expression.ToCsNode<ExpressionSyntax>(),
                       csName))
                   .AddArgumentListArguments();
            }
            else
            {
                return SyntaxFactory.BinaryExpression(
                    SyntaxKind.AsExpression,
                    node.Expression.ToCsNode<ExpressionSyntax>(),
                    node.Type.ToCsNode<ExpressionSyntax>());
            }
        }
    }
}

