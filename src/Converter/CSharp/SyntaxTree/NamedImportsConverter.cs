using TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class NamedImportsConverter : NodeConverter
    {
        public SyntaxList<UsingDirectiveSyntax> Convert(NamedImports node)
        {
            return new SyntaxList<UsingDirectiveSyntax>(node.Elements.ToCsSyntaxTrees<UsingDirectiveSyntax>());
        }
    }
}
