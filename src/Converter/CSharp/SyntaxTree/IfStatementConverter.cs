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
    public class IfStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(IfStatement node)
        {
            IfStatementSyntax csIfStatement = SyntaxFactory.IfStatement(
                node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                node.ThenStatement.ToCsSyntaxTree<StatementSyntax>());

            if (node.ElseStatement != null)
            {
                csIfStatement = csIfStatement.WithElse(SyntaxFactory.ElseClause(node.ElseStatement.ToCsSyntaxTree<StatementSyntax>()));
            }

            return csIfStatement;
        }
    }
}

