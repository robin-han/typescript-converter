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
    public class TupleTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(TupleType node) // let a: [number, string] = [1, "hello"];
        {
            if (this.Context.Config.PreferTypeScriptType)
            {
                return SyntaxFactory
                    .GenericName("Array")
                    .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("Object"));
            }
            else
            {
                return SyntaxFactory
                    .GenericName("List")
                    .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("object"));
            }
        }
    }

}