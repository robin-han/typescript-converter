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
    public class CaseClauseConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(CaseClause node)
        {
            SwitchSectionSyntax csSwitchSection = SyntaxFactory.SwitchSection();

            csSwitchSection = csSwitchSection.AddLabels(SyntaxFactory.CaseSwitchLabel(node.Expression.ToCsSyntaxTree<ExpressionSyntax>()));
            csSwitchSection = csSwitchSection.AddStatements(node.Statements.ToCsSyntaxTrees<StatementSyntax>());

            return csSwitchSection;
        }
    }
}

