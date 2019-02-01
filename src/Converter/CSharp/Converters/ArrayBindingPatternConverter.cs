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
    public class ArrayBindingPatternConverter : Converter
    {
        public CSharpSyntaxNode Convert(ArrayBindingPattern node)
        {
            //TODO: NOT SUPPORT:   (sender: any, args: EventArgs): void;
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }
    }
}

