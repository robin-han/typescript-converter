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
    public class AnyKeywordConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(AnyKeyword node)
        {
            return SyntaxFactory.IdentifierName("dynamic");
            
            //if (this.Context.PreferTypeScriptType)
            //{
            //    return SyntaxFactory.IdentifierName("Any");
            //}
            //else
            //{
            //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));
            //}
        }
    }
}

