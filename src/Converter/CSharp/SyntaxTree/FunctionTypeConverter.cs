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
    public class FunctionTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(FunctionType node)
        {
            //TODO: function find<T>(array: T[], callbackfn: (value: T) => boolean): T {}, need to create delegate declaration.
            return SyntaxFactory.IdentifierName(this.CommentText(node.Text));
        }
    }
}

