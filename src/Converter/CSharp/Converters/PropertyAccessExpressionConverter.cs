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

            // omit this
            if (node.Expression.Kind == NodeKind.ThisKeyword && WithInStaticMethod(node))
            {
                return csName;
            }
            // omit names(dv. core.)
            List<string> omittedNames = LangConverter.CurrentContext.OmittedQualifiedNames;
            if (omittedNames.Count > 0 && omittedNames.Contains(node.Expression.Text.Trim()))
            {
                return csName;
            }
            //
            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node.Expression.ToCsNode<ExpressionSyntax>(), csName);
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

            MethodDeclaration method = parent as MethodDeclaration;
            return (method == null ? false : method.IsStatic);
        }

    }
}

