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
    public class LiteralTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(LiteralType node)
        {
            //TODO: NOT SUPPORT
            //return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
            return SyntaxFactory.IdentifierName("dynamic");
        }
    }
}

