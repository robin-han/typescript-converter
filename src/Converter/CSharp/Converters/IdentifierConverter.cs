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
    public class IdentifierConverter : Converter
    {
        public CSharpSyntaxNode Convert(Identifier node)
        {
            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                return SyntaxFactory
                   .GenericName(node.Text)
                   .AddTypeArgumentListArguments(typeArguments.ToCsNodes<TypeSyntax>());
            }
            return SyntaxFactory.IdentifierName(node.Text);
        }
    }
}

