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

            csCompilationUnit = csCompilationUnit.AddMembers(sourceFile.MouduleDeclarations.ToCsNodes<MemberDeclarationSyntax>());
            foreach (Node import in sourceFile.ImportDeclarations)
            {
                foreach (UsingDirectiveSyntax usingSyntax in import.ToCsNode<SyntaxList<UsingDirectiveSyntax>>())
                {
                    csCompilationUnit = csCompilationUnit.AddUsings(usingSyntax);
                }
            }

            List<Node> typeAliases = sourceFile.TypeAliases;
            List<Node> typeDefinitions = this.FilterTypes(sourceFile.TypeDefinitions);
            if (typeAliases.Count > 0 || typeDefinitions.Count > 0)
            {
                string ns = sourceFile.Document.GetPackageName();
                if (!string.IsNullOrEmpty(ns))
                {
                    NamespaceDeclarationSyntax nsSyntaxNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns));
                    nsSyntaxNode = nsSyntaxNode
                        .AddUsings(typeAliases.ToCsNodes<UsingDirectiveSyntax>())
                        .AddMembers(typeDefinitions.ToCsNodes<MemberDeclarationSyntax>());
                    csCompilationUnit = csCompilationUnit.AddMembers(nsSyntaxNode);
                }
                else
                {
                    csCompilationUnit = csCompilationUnit
                        .AddUsings(typeAliases.ToCsNodes<UsingDirectiveSyntax>())
                        .AddMembers(typeDefinitions.ToCsNodes<MemberDeclarationSyntax>());
                }
            }

            return csCompilationUnit;
        }
    }
}

