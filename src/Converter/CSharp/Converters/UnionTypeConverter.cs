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
    public class UnionTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(UnionType node)
        {
            //TODO: NOT SUPPORT
            //return SyntaxFactory.ParseName(this.CommentText(node.Text));
            return SyntaxFactory.IdentifierName("dynamic");
        }
    }
}

