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
    public class QualifiedNameConverter : Converter
    {
        public CSharpSyntaxNode Convert(QualifiedName node)
        {
            NameSyntax csLeft = node.Left.ToCsNode<NameSyntax>();
            SimpleNameSyntax csRight = null;

            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                csRight = SyntaxFactory
                    .GenericName(node.Right.Text)
                    .AddTypeArgumentListArguments(typeArguments.ToCsNodes<TypeSyntax>());
            }
            else
            {
                csRight = node.Right.ToCsNode<SimpleNameSyntax>();
            }

            return SyntaxFactory.QualifiedName(csLeft, csRight);
        }
    }
}

