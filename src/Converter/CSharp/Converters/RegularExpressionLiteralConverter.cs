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
    public class RegularExpressionLiteralConverter : Converter
    {
		public CSharpSyntaxNode Convert(RegularExpressionLiteral node)
        {
            return SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName("Regex"))
                .AddArgumentListArguments(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(node.Pattern))); 
        }
    }
}

