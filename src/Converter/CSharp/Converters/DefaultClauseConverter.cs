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
    public class DefaultClauseConverter : Converter
    {
        public CSharpSyntaxNode Convert(DefaultClause node)
        {
            SwitchSectionSyntax csSwitchSection = SyntaxFactory.SwitchSection();

            csSwitchSection = csSwitchSection.AddLabels(SyntaxFactory.DefaultSwitchLabel());
            csSwitchSection = csSwitchSection.AddStatements(node.Statements.ToCsNodes<StatementSyntax>());

            return csSwitchSection;
        }
    }
}

