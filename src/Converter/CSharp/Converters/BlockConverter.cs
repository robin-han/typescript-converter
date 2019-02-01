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
    public class BlockConverter : Converter
    {
        public CSharpSyntaxNode Convert(Block node)
        {
            return SyntaxFactory.Block(node.Statements.ToCsNodes<StatementSyntax>());
        }
    }
}

