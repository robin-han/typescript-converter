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
    public class IntersectionTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(IntersectionType node)
        {
            var ret = new List<TypeSyntax>();
            foreach (var type in node.Types)
            {
                ret.Add(type.ToCsSyntaxTree<TypeSyntax>());
            }
            return SyntaxFactory
                    .GenericName("Combine")
                    .AddTypeArgumentListArguments(ret.ToArray());
        }
    }
}
