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
    public class NumberKeywordConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(NumberKeyword node)
        {
            //if (this.Context.Config.PreferTypeScriptType)
            //{
            //    return SyntaxFactory.IdentifierName("Number");
            //}
            //else
            //{
            //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));
            //}
            return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));
        }

    }
}

