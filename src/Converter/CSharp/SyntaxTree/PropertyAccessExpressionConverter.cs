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
    public class PropertyAccessExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(PropertyAccessExpression node)
        {
            Node parent = node.Parent;
            if (parent != null && parent.Kind == NodeKind.EnumMember && node.Text == "Number.MAX_VALUE")
            {
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                    SyntaxFactory.IdentifierName("MaxValue"));
            }

            SimpleNameSyntax csName = null;
            if (parent != null)
            {
                Node expression = parent.GetValue("Expression") as Node;

                List<Node> typeArguments;
                if (parent is CallExpression callExpr && CallExpressionConverter.IsAsEnumInvoke(callExpr))
                {
                    typeArguments = new List<Node>() { callExpr.Arguments[1] };
                }
                else
                {
                    typeArguments = parent.GetValue("TypeArguments") as List<Node>;
                }
                if (expression != null && expression == node && typeArguments != null && typeArguments.Count > 0)
                {
                    csName = SyntaxFactory
                   .GenericName(node.Name.Text)
                   .AddTypeArgumentListArguments(typeArguments.ToCsSyntaxTrees<TypeSyntax>());
                }
            }
            if (csName == null)
            {
                csName = node.Name.ToCsSyntaxTree<SimpleNameSyntax>();
            }

            // omit this
            if (node.Expression.Kind == NodeKind.ThisKeyword && WithInStaticMethod(node))
            {
                return csName;
            }
            // omit names(dv. core.)
            if (this.IsQualifiedName(node.Expression.Text))
            {
                return csName;
            }
            //
            MemberAccessExpressionSyntax accessExprSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node.Expression.ToCsSyntaxTree<ExpressionSyntax>(), csName);
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

