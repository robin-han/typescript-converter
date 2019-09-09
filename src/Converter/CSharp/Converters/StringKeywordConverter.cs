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
    public class StringKeywordConverter : Converter
    {
        public CSharpSyntaxNode Convert(StringKeyword node)
        {
            if (this.Context.Config.PreferTypeScriptType)
            {
                return SyntaxFactory.IdentifierName("String");
            }
            else
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
            }
        }
    }
}

