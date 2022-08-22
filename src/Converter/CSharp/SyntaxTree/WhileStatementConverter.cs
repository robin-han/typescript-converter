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
    public class WhileStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(WhileStatement node)
        {
            return SyntaxFactory.WhileStatement(
                node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                node.Statement.ToCsSyntaxTree<StatementSyntax>());
        }
    }
}

