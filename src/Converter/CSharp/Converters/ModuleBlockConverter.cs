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
    public class ModuleBlockConverter : Converter
    {
        public SyntaxList<MemberDeclarationSyntax> Convert(ModuleBlock node)
        {
            List<Node> statements = this.FilterStatements(node.Statements);
            return SyntaxFactory.List(statements.ToCsNodes<MemberDeclarationSyntax>());
        }
    }
}

