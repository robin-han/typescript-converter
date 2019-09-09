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
    public class PropertyAccessExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(PropertyAccessExpression node)
        {
            if (node.Parent != null && node.Parent.Kind == NodeKind.EnumMember && node.Text.Trim() == "Number.MAX_VALUE")
            {
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                    SyntaxFactory.IdentifierName("MaxValue"));
            }

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

            // omit this
            if (node.Expression.Kind == NodeKind.ThisKeyword && WithInStaticMethod(node))
            {
                return csName;
            }
            // omit names(dv. core.)
            List<string> omittedNames = this.Context.Config.OmittedQualifiedNames;
            if (omittedNames.Count > 0 && omittedNames.Contains(node.Expression.Text.Trim()))
            {
                return csName;
            }
            //
            MemberAccessExpressionSyntax accessExprSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node.Expression.ToCsNode<ExpressionSyntax>(), csName);
            return this.As(accessExprSyntax, node.As);
        }

        private bool WithInStaticMethod(PropertyAccessExpression node)
        {
            Node parent = node.Parent;
            while (parent != null)
            {
                if (parent.Kind == NodeKind.MethodDeclaration || parent.Kind == NodeKind.ClassDeclaration)
                {
                    break;
                }
                parent = parent.Parent;
            }

            return (!(parent is MethodDeclaration method) ? false : method.IsStatic);
        }

    }
}

