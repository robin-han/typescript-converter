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
    public class ReturnStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(ReturnStatement node)
        {
            if (node.Expression == null)
            {
                return SyntaxFactory.ReturnStatement();
            }
            return SyntaxFactory.ReturnStatement(node.Expression.ToCsNode<ExpressionSyntax>());
        }
    }
}

