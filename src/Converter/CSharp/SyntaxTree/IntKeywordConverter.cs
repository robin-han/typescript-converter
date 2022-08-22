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
    public class IntKeywordConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(IntKeyword node)
        {
            return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));
        }
    }
}

