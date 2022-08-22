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
    public class SourceFileConverter : NodeConverter
    {
        private const string STATIC_USING = "static ";
        private const string EQUALS_USING = " = ";
        public CSharpSyntaxNode Convert(SourceFile sourceFile)
        {
            CompilationUnitSyntax csCompilationUnit = SyntaxFactory.CompilationUnit();
            foreach (string us in this.Context.Usings)
            {
                if (us.StartsWith(STATIC_USING))
                {
                    string name = us.Substring(STATIC_USING.Length);
                    csCompilationUnit = csCompilationUnit.AddUsings(
                        SyntaxFactory.UsingDirective(SyntaxFactory.Token(SyntaxKind.StaticKeyword), null, SyntaxFactory.ParseName(name))
                    );
                }
                else if (us.Contains(EQUALS_USING))
                {
                    int index = us.IndexOf(EQUALS_USING);
                    string equalsName = us.Substring(0, index);
                    string name = us.Substring(index + EQUALS_USING.Length);
                    csCompilationUnit = csCompilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.NameEquals(equalsName), SyntaxFactory.ParseName(name)));
                }
                else
                {
                    csCompilationUnit = csCompilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(us)));
                }
            }

            csCompilationUnit = csCompilationUnit.AddMembers(sourceFile.ModuleDeclarations.ToCsSyntaxTrees<MemberDeclarationSyntax>());
            foreach (Node import in sourceFile.ImportDeclarations)
            {
                foreach (UsingDirectiveSyntax usingSyntax in import.ToCsSyntaxTree<SyntaxList<UsingDirectiveSyntax>>())
                {
                    csCompilationUnit = csCompilationUnit.AddUsings(usingSyntax);
                }
            }

            List<Node> typeAliases = sourceFile.TypeAliases;
            List<Node> typeDefinitions = this.FilterTypes(sourceFile.GetTypeDeclarations());
            if (typeAliases.Count > 0 || typeDefinitions.Count > 0)
            {
                string ns = sourceFile.Document.GetPackageName();
                if (!string.IsNullOrEmpty(ns))
                {
                    NamespaceDeclarationSyntax nsSyntaxNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns));
                    nsSyntaxNode = nsSyntaxNode
                        .AddUsings(typeAliases.ToCsSyntaxTrees<UsingDirectiveSyntax>())
                        .AddMembers(typeDefinitions.ToCsSyntaxTrees<MemberDeclarationSyntax>());
                    csCompilationUnit = csCompilationUnit.AddMembers(nsSyntaxNode);
                }
                else
                {
                    csCompilationUnit = csCompilationUnit
                        .AddUsings(typeAliases.ToCsSyntaxTrees<UsingDirectiveSyntax>())
                        .AddMembers(typeDefinitions.ToCsSyntaxTrees<MemberDeclarationSyntax>());
                }
            }

            return csCompilationUnit;
        }
    }
}

