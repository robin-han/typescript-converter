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
    public class TryStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(TryStatement node)
        {
            TryStatementSyntax csTryStatement = SyntaxFactory.TryStatement().WithBlock(node.TryBlock.ToCsNode<BlockSyntax>());

            if (node.CatchClause != null)
            {
                csTryStatement = csTryStatement.AddCatches(node.CatchClause.ToCsNode<CatchClauseSyntax>());
            }
            if (node.FinallyBlock != null)
            {
                csTryStatement = csTryStatement.WithFinally(SyntaxFactory.FinallyClause(node.FinallyBlock.ToCsNode<BlockSyntax>()));
            }

            return csTryStatement;
        }

    }
}

