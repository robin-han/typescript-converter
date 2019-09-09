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
    public class CallSignatureConverter : Converter
    {
        public CSharpSyntaxNode Convert(CallSignature node)
        {
            //TODO: NOT SUPPORT
            return SyntaxFactory.ParseStatement(this.CommentText(node.Text));
        }
    }
}

