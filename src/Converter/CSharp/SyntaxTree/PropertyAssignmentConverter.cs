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
    public class PropertyAssignmentConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(PropertyAssignment node)
        {
            return SyntaxFactory
                .Argument(node.Initializer.ToCsSyntaxTree<ExpressionSyntax>())
                .WithNameColon(SyntaxFactory.NameColon(node.Name.Text));
        }
    }
}

