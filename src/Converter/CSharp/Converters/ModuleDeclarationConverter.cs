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
    public class ModuleDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(ModuleDeclaration module)
        {
            ModuleBlock mb = this.GetModuleBlock(module);
            if (mb == null)
            {
                return SyntaxFactory
                    .NamespaceDeclaration(SyntaxFactory.ParseName(module.Name.Text))
                    .AddMembers(module.Body.ToCsNode<MemberDeclarationSyntax>());
            }

            string ns = string.Empty;
            if (!string.IsNullOrEmpty(this.Context.Config.Namespace))
            {
                ns = this.Context.Config.Namespace;
            }
            else
            {
                ns = this.GetNamespace(module);
                if (this.Context.Config.NamespaceMappings.ContainsKey(ns))
                {
                    ns = this.Context.Config.NamespaceMappings[ns];
                }
            }
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns))
                .AddUsings(mb.TypeAliases.ToCsNodes<UsingDirectiveSyntax>())
                .WithMembers(mb.ToCsNode<SyntaxList<MemberDeclarationSyntax>>());
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

        private ModuleBlock GetModuleBlock(ModuleDeclaration module)
        {
            ModuleDeclaration md = module;
            while (md != null)
            {
                if (md.Body.Kind == NodeKind.ModuleBlock)
                {
                    return md.Body as ModuleBlock;
                }
                md = md.Body as ModuleDeclaration;
            }
            return null;
        }
    }
}

