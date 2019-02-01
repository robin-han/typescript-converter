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
    public class VariableDeclarationListConverter : Converter
    {
        public CSharpSyntaxNode Convert(VariableDeclarationList node)
        {
            return SyntaxFactory
                .VariableDeclaration(node.Type.ToCsNode<TypeSyntax>())
                .AddVariables(node.Declarations.ToCsNodes<VariableDeclaratorSyntax>());
        }
    }
}

