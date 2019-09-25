using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;
using Microsoft.CodeAnalysis;

namespace TypeScript.Converter.CSharp
{
    public class SourceFileConverter : Converter
    {
        public CSharpSyntaxNode Convert(SourceFile sourceFile)
        {
            CompilationUnitSyntax csCompilationUnit = SyntaxFactory.CompilationUnit();
            foreach (string us in this.Context.Config.Usings)
            {
                csCompilationUnit = csCompilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(us)));
            }

            List<Node> typeAliasStatements = new List<Node>();
            List<Node> typeStatements = new List<Node>();
            foreach (Node statement in this.FilterStatements(sourceFile.Statements))
            {
                switch (statement.Kind)
                {
                    case NodeKind.ClassDeclaration:
                    case NodeKind.InterfaceDeclaration:
                    case NodeKind.EnumDeclaration:
                        typeStatements.Add(statement);
                        break;

                    case NodeKind.TypeAliasDeclaration:
                        typeAliasStatements.Add(statement);
                        break;

                    case NodeKind.ImportDeclaration:
                        foreach (UsingDirectiveSyntax usingSyntax in statement.ToCsNode<SyntaxList<UsingDirectiveSyntax>>())
                        {
                            csCompilationUnit = csCompilationUnit.AddUsings(usingSyntax);
                        }
                        break;

                    case NodeKind.ExportDeclaration:
                    case NodeKind.ExportAssignment:
                        break;

                    default:
                        csCompilationUnit = csCompilationUnit.AddMembers(statement.ToCsNode<MemberDeclarationSyntax>());
                        break;
                }
            }

            bool isExternalModule = (typeAliasStatements.Count > 0 || typeStatements.Count > 0);
            if (isExternalModule)
            {
                string ns = sourceFile.Document.GetPackageName();
                if (!string.IsNullOrEmpty(ns))
                {
                    NamespaceDeclarationSyntax nsSyntaxNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns));
                    nsSyntaxNode = nsSyntaxNode
                        .AddUsings(typeAliasStatements.ToCsNodes<UsingDirectiveSyntax>())
                        .AddMembers(typeStatements.ToCsNodes<MemberDeclarationSyntax>());
                    csCompilationUnit = csCompilationUnit.AddMembers(nsSyntaxNode);
                }
                else
                {
                    csCompilationUnit = csCompilationUnit
                        .AddUsings(typeAliasStatements.ToCsNodes<UsingDirectiveSyntax>())
                        .AddMembers(typeStatements.ToCsNodes<MemberDeclarationSyntax>());
                }
            }

            return csCompilationUnit;
        }
    }
}

