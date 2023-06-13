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
    public class ShorthandPropertyAssignmentConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ShorthandPropertyAssignment node)
        {
            return SyntaxFactory
                .Argument(SyntaxFactory.ParseName(NormalizeTypeName(node.Name)))
                .WithNameColon(SyntaxFactory.NameColon(NormalizeTypeName(node.Name)));
        }
    }
}
