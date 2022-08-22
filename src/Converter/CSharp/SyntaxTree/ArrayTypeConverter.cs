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
    public class ArrayTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ArrayType node)
        {
            return Convert(node, node.ElementType.ToCsSyntaxTree<TypeSyntax>(), this.Context.TypeScriptType);
        }

        internal static CSharpSyntaxNode Convert(ArrayType node, TypeSyntax elementType, bool typeScript)
        {
            if (typeScript)
            {
                return SyntaxFactory
                    .GenericName("Array")
                    .AddTypeArgumentListArguments(elementType);
            }
            else
            {
                return SyntaxFactory
                    .GenericName("List")
                    .AddTypeArgumentListArguments(elementType);
            }
        }
    }
}

