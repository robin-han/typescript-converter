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
    public class ForInStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ForInStatement node)
        {
            var init = node.Initializer as VariableDeclarationList;
            if (init == null || init.Declarations.Count != 1)
            {
                //TODO: not support multiple vars for now
                return SyntaxFactory.ParseStatement(this.CommentText(node.Text));
            }

            var varName = init.Declarations[0] as VariableDeclaration;
            return SyntaxFactory.ForEachStatement(
                SyntaxFactory.IdentifierName("var"),
                NormalizeTypeName(varName.Name),
                node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                node.Statement.ToCsSyntaxTree<StatementSyntax>());
        }
    }
}
