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
    public class PrivateKeywordConverter : Converter
    {
        public SyntaxToken Convert(PrivateKeyword node)
        {
            return SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
        }
    }
}

