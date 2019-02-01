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
    public class ForStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(ForStatement node)
        {
            ForStatementSyntax csForStatement = SyntaxFactory.ForStatement(node.Statement.ToCsNode<StatementSyntax>());

            Node initializer = node.Initializer;
            if (initializer != null)
            {
                if (initializer.Kind == NodeKind.VariableDeclarationList)
                {
                    csForStatement = csForStatement.WithDeclaration(initializer.ToCsNode<VariableDeclarationSyntax>());
                }
                else
                {
                    csForStatement = csForStatement.AddInitializers(node.Initializers.ToCsNodes<ExpressionSyntax>());
                }
            }

            if (node.Condition != null)
            {
                csForStatement = csForStatement.WithCondition(node.Condition.ToCsNode<ExpressionSyntax>());
            }

            if (node.Incrementors != null)
            {
                csForStatement = csForStatement.AddIncrementors(node.Incrementors.ToCsNodes<ExpressionSyntax>());
            }

            return csForStatement;
        }

    }
}

