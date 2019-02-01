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
    public class IfStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(IfStatement node)
        {
            IfStatementSyntax csIfStatement = SyntaxFactory.IfStatement(
                node.Expression.ToCsNode<ExpressionSyntax>(),
                node.ThenStatement.ToCsNode<StatementSyntax>());

            if (node.ElseStatement != null)
            {
                csIfStatement = csIfStatement.WithElse(SyntaxFactory.ElseClause(node.ElseStatement.ToCsNode<StatementSyntax>()));
            }

            return csIfStatement;
        }
    }
}

