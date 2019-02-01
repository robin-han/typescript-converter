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
    public class ModuleDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(ModuleDeclaration node)
        {
            NamespaceDeclarationSyntax csNS = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(node.Name.Text));

            if (node.Body is ModuleBlock mb)
            {
                csNS = csNS.WithMembers(mb.ToCsNode<SyntaxList<MemberDeclarationSyntax>>());
                csNS = csNS.AddUsings(mb.TypeAliases.ToCsNodes<UsingDirectiveSyntax>());
            }
            else
            {
                csNS = csNS.AddMembers(node.Body.ToCsNode<MemberDeclarationSyntax>());
            }

            return csNS;
        }
    }
}

