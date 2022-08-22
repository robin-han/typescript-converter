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
    public class TryStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TryStatement node)
        {
            TryStatementSyntax csTryStatement = SyntaxFactory.TryStatement().WithBlock(node.TryBlock.ToCsSyntaxTree<BlockSyntax>());

            if (node.CatchClause != null)
            {
                csTryStatement = csTryStatement.AddCatches(node.CatchClause.ToCsSyntaxTree<CatchClauseSyntax>());
            }
            if (node.FinallyBlock != null)
            {
                csTryStatement = csTryStatement.WithFinally(SyntaxFactory.FinallyClause(node.FinallyBlock.ToCsSyntaxTree<BlockSyntax>()));
            }

            return csTryStatement;
        }

    }
}

