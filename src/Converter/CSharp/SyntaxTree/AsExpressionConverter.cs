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
    public class AsExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(AsExpression node)
        {
            if (TypeHelper.IsArrayType(node.Type)) //to .AsArray<T>()
            {
                GenericNameSyntax csName = SyntaxFactory.GenericName("AsArray");

                if (node.Type.Kind == NodeKind.ArrayType)
                {
                    csName = csName.AddTypeArgumentListArguments((node.Type as ArrayType).ElementType.ToCsSyntaxTree<TypeSyntax>());
                }
                else if (node.Type.Kind == NodeKind.TypeReference)
                {
                    csName = csName.AddTypeArgumentListArguments((node.Type as TypeReference).TypeArguments[0].ToCsSyntaxTree<TypeSyntax>());
                }

                return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                        csName))
                    .AddArgumentListArguments();
            }
            else if (TypeHelper.ToShortName(node.Type.Text) == "DataValueType")
            {
                GenericNameSyntax csName = SyntaxFactory.GenericName("As");
                csName = csName.AddTypeArgumentListArguments(node.Type.ToCsSyntaxTree<TypeSyntax>());

                return SyntaxFactory.InvocationExpression(
                   SyntaxFactory.MemberAccessExpression(
                       SyntaxKind.SimpleMemberAccessExpression,
                       node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                       csName))
                   .AddArgumentListArguments();
            }
            else
            {
                return SyntaxFactory.BinaryExpression(
                    SyntaxKind.AsExpression,
                    node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                    node.Type.ToCsSyntaxTree<ExpressionSyntax>());
            }
        }
    }
}

