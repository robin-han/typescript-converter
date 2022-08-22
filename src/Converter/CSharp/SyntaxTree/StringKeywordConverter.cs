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
    public class StringKeywordConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(StringKeyword node)
        {
            if (this.Context.TypeScriptType)
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

