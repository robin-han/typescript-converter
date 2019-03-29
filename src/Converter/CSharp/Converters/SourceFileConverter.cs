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
    public class SourceFileConverter : Converter
    {
        public CSharpSyntaxNode Convert(SourceFile node)
        {
            CompilationUnitSyntax csCompilationUnit = SyntaxFactory.CompilationUnit();

            foreach (string us in this.Context.Config.Usings)
            {
                csCompilationUnit = csCompilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(us)));
            }

            return csCompilationUnit.AddMembers(node.Statements.ToCsNodes<MemberDeclarationSyntax>());
        }
    }
}

