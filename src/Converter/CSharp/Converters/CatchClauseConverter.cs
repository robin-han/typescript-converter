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
    public class CatchClauseConverter : Converter
    {
        public CSharpSyntaxNode Convert(CatchClause node)
        {
            CatchDeclarationSyntax csCatchDeclaration = SyntaxFactory.CatchDeclaration(
                SyntaxFactory.IdentifierName(node.VariableDeclaration.Type.Text),
                SyntaxFactory.Identifier(node.VariableDeclaration.Name.Text));

            BlockSyntax csCatchBlock = node.Block.ToCsNode<BlockSyntax>();

            return SyntaxFactory.CatchClause().WithDeclaration(csCatchDeclaration).WithBlock(csCatchBlock);
        }
    }
}

