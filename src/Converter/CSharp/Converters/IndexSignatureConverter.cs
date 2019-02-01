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
    public class IndexSignatureConverter : Converter
    {
        public CSharpSyntaxNode Convert(IndexSignature node)
        {
            return SyntaxFactory
                .GenericName("Hashtable")
                .AddTypeArgumentListArguments(node.KeyType.ToCsNode<TypeSyntax>(), node.Type.ToCsNode<TypeSyntax>());
        }
    }
}

