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
    public class ModuleBlockConverter : Converter
    {
        public SyntaxList<MemberDeclarationSyntax> Convert(ModuleBlock node)
        {
            List<Node> statements = this.Filter(node.Statements);
            return SyntaxFactory.List(statements.ToCsNodes<MemberDeclarationSyntax>());
        }

        private List<Node> Filter(List<Node> statements)
        {
            List<string> excluteTypes = this.Context.Config.ExcludeTypes;
            if (excluteTypes.Count == 0)
            {
                return statements;
            }

            return statements.FindAll(statement =>
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        return !excluteTypes.Contains((statement as ClassDeclaration).Name.Text);

                    case NodeKind.InterfaceDeclaration:
                        return !excluteTypes.Contains((statement as InterfaceDeclaration).Name.Text);

                    case NodeKind.EnumDeclaration:
                        return !excluteTypes.Contains((statement as EnumDeclaration).Name.Text);

                    default:
                        return true;
                }
            });
        }

    }
}

