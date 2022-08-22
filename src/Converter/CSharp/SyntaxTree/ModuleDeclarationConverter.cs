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
    public class ModuleDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ModuleDeclaration module)
        {
            string ns = this.Context.Namespace;
            if (string.IsNullOrEmpty(ns))
            {
                ns = this.GetNamespace(module);
            }

            NamespaceDeclarationSyntax nsSyntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns));
            ModuleBlock mb = module.GetModuleBlock();
            if (mb != null)
            {
                nsSyntax = nsSyntax
                    .AddUsings(mb.TypeAliases.ToCsSyntaxTrees<UsingDirectiveSyntax>())
                    .WithMembers(mb.ToCsSyntaxTree<SyntaxList<MemberDeclarationSyntax>>());
            }
            return nsSyntax;
        }

        private string GetNamespace(ModuleDeclaration module)
        {
            List<string> parts = new List<string>();
            ModuleDeclaration md = module;
            while (md != null)
            {
                parts.Add(md.Name.Text);
                md = md.Body as ModuleDeclaration;
            }
            return string.Join('.', parts);
        }
    }
}

