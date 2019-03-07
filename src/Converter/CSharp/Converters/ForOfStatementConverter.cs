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
    public class ForOfStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(ForOfStatement node)
        {
            Node type = node.CreateNode(NodeKind.Identifier, "var");
            return SyntaxFactory.ForEachStatement(
                type.ToCsNode<TypeSyntax>(),
                node.Identifier.Text,
                node.Expression.ToCsNode<ExpressionSyntax>(),
                node.Statement.ToCsNode<StatementSyntax>());
        }
    }
}

