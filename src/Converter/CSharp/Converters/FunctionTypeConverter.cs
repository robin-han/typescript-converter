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
    public class FunctionTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(FunctionType node)
        {
            //TODO: function find<T>(array: T[], callbackfn: (value: T) => boolean): T {}, need to create delegate declaration.
            return SyntaxFactory.IdentifierName(this.CommentText(node.Text));
        }
    }
}

