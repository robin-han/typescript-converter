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
    public class QualifiedNameConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(QualifiedName node)
        {
            SimpleNameSyntax csRight = null;
            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                csRight = SyntaxFactory
                    .GenericName(node.Right.Text)
                    .AddTypeArgumentListArguments(typeArguments.ToCsSyntaxTrees<TypeSyntax>());
            }
            else
            {
                csRight = node.Right.ToCsSyntaxTree<SimpleNameSyntax>();
            }

            // omit names(dv. core.)
            if (this.IsQualifiedName(node.Left.Text))
            {
                return csRight;
            }
            // 
            return SyntaxFactory.QualifiedName(node.Left.ToCsSyntaxTree<NameSyntax>(), csRight);
        }
    }
}

