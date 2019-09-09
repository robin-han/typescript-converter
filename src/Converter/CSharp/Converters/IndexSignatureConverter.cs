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
    public class IndexSignatureConverter : Converter
    {
        public CSharpSyntaxNode Convert(IndexSignature node)
        {
            if (this.Context.Config.PreferTypeScriptType)
            {
                return SyntaxFactory
                    .GenericName("Hashtable")
                    .AddTypeArgumentListArguments(node.KeyType.ToCsNode<TypeSyntax>(), node.Type.ToCsNode<TypeSyntax>());
            }
            else
            {
                return SyntaxFactory
                    .GenericName("Dictionary")
                    .AddTypeArgumentListArguments(node.KeyType.ToCsNode<TypeSyntax>(), node.Type.ToCsNode<TypeSyntax>());
            }
        }
    }
}

