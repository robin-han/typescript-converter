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
    public class ForInStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ForInStatement node)
        {
            //TODO: not support now
            return SyntaxFactory.ParseStatement(this.CommentText(node.Text));
        }
    }
}

