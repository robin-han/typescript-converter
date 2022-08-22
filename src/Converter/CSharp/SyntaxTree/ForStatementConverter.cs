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
    public class ForStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ForStatement node)
        {
            ForStatementSyntax csForStatement = SyntaxFactory.ForStatement(node.Statement.ToCsSyntaxTree<StatementSyntax>());

            Node initializer = node.Initializer;
            if (initializer != null)
            {
                if (initializer.Kind == NodeKind.VariableDeclarationList)
                {
                    csForStatement = csForStatement.WithDeclaration(initializer.ToCsSyntaxTree<VariableDeclarationSyntax>());
                }
                else
                {
                    csForStatement = csForStatement.AddInitializers(node.Initializers.ToCsSyntaxTrees<ExpressionSyntax>());
                }
            }

            if (node.Condition != null)
            {
                csForStatement = csForStatement.WithCondition(node.Condition.ToCsSyntaxTree<ExpressionSyntax>());
            }

            if (node.Incrementors != null)
            {
                csForStatement = csForStatement.AddIncrementors(node.Incrementors.ToCsSyntaxTrees<ExpressionSyntax>());
            }

            return csForStatement;
        }

    }
}

