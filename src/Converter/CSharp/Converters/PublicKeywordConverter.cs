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
    public class PublicKeywordConverter : Converter
    {
        public SyntaxToken Convert(PublicKeyword node)
        {
            return SyntaxFactory.Token(SyntaxKind.PublicKeyword);
        }
    }
}

