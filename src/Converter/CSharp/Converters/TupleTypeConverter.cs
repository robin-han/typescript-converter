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
    public class TupleTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(TupleType node) // let a: [number, string] = [1, "hello"];
        {
            return SyntaxFactory
                .GenericName("Array")
                .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("Object"));
        }
    }

}