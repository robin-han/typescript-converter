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
    public class ModuleBlockConverter : NodeConverter
    {
        public SyntaxList<MemberDeclarationSyntax> Convert(ModuleBlock node)
        {
            List<Node> typeNodes = this.FilterTypes(node.TypeDeclarations);
            return SyntaxFactory.List(typeNodes.ToCsSyntaxTrees<MemberDeclarationSyntax>());
        }
    }
}

