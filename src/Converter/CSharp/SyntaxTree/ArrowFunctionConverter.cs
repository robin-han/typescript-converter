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
    public class ArrowFunctionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ArrowFunction node)
        {
            return SyntaxFactory
                .ParenthesizedLambdaExpression(node.Body.ToCsSyntaxTree<CSharpSyntaxNode>())
                .AddParameterListParameters(node.Parameters.ToCsSyntaxTrees<ParameterSyntax>());
        }
    }
}

