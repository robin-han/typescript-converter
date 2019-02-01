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
    public class SwitchStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(SwitchStatement node)
        {
            return SyntaxFactory
                .SwitchStatement(node.Expression.ToCsNode<ExpressionSyntax>())
                .WithSections(node.CaseBlock.ToCsNode<SyntaxList<SwitchSectionSyntax>>());
        }
    }
}

