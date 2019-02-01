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
    public class NumberKeywordConverter : Converter
    {
        public CSharpSyntaxNode Convert(NumberKeyword node)
        {
            //PredefinedTypeSyntax csType;
            //if (node.Integer)
            //{
            //    csType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));
            //}
            //else
            //{
            //    csType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));
            //}

            //if (node.Nullable)
            //{
            //    return SyntaxFactory.NullableType(csType);
            //}
            //return csType;

            return SyntaxFactory.IdentifierName("Number");
        }

    }
}

