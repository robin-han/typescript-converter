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
    public class CatchClauseConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(CatchClause node)
        {
            BlockSyntax csCatchBlock = node.Block.ToCsSyntaxTree<BlockSyntax>();

            if (node.VariableDeclaration == null)
            {
                return SyntaxFactory.CatchClause().WithBlock(csCatchBlock);
            }
            else
            {
                CatchDeclarationSyntax csCatchDeclaration = SyntaxFactory.CatchDeclaration(
                   SyntaxFactory.IdentifierName(node.VariableDeclaration.Type.Text),
                   SyntaxFactory.Identifier(node.VariableDeclaration.Name.Text));

                return SyntaxFactory.CatchClause().WithDeclaration(csCatchDeclaration).WithBlock(csCatchBlock);
            }
        }
    }
}

