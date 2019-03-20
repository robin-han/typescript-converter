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
    public class BooleanKeywordConverter : Converter
    {
        public CSharpSyntaxNode Convert(BooleanKeyword node)
        {
            ConverterContext context = LangConverter.CurrentContext;

            if (context.PreferTypeScriptType)
            {
                return SyntaxFactory.IdentifierName("Boolean");
            }
            else
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));
            }
        }
    }
}

