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
    public class IndexSignatureConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(IndexSignature node)
        {
            if (this.Context.TypeScriptType)
            {
                return SyntaxFactory
                    .GenericName("Hashtable")
                    .AddTypeArgumentListArguments(node.KeyType.ToCsSyntaxTree<TypeSyntax>(), node.Type.ToCsSyntaxTree<TypeSyntax>());
            }
            else
            {
                return SyntaxFactory
                    .GenericName("Dictionary")
                    .AddTypeArgumentListArguments(node.KeyType.ToCsSyntaxTree<TypeSyntax>(), node.Type.ToCsSyntaxTree<TypeSyntax>());
            }
        }
    }
}

