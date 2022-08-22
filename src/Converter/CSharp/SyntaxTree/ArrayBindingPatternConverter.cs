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
    public class ArrayBindingPatternConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ArrayBindingPattern node)
        {
            //TODO: NOT SUPPORT:   (sender: any, args: EventArgs): void;
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }
    }
}

