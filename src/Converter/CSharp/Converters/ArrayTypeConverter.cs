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
    public class ArrayTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(ArrayType node)
        {
            if (node.IsParams)
            {
                return SyntaxFactory
                    .ArrayType(node.ElementType.ToCsNode<TypeSyntax>())
                    .AddRankSpecifiers(SyntaxFactory.ArrayRankSpecifier());
            }
            else
            {
                return SyntaxFactory
                    .GenericName("Array")
                    .AddTypeArgumentListArguments(node.ElementType.ToCsNode<TypeSyntax>());
            }
        }
    }
}

