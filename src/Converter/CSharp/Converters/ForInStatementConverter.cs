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
    public class ForInStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(ForInStatement node)
        {
            //TODO: not support now
            return SyntaxFactory.ParseStatement(this.CommentText(node.Text));
        }
    }
}

