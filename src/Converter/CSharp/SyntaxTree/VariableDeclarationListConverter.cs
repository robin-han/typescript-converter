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
    public class VariableDeclarationListConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(VariableDeclarationList node)
        {
            bool isVar = false;
            Node type = node.Type;
            if (type.Kind == NodeKind.AnyKeyword && node.Declarations.Count > 0)
            {
                VariableDeclaration variableNode = node.Declarations[0] as VariableDeclaration;
                if (variableNode.Initializer != null && variableNode.Initializer.Kind != NodeKind.NullKeyword)
                {
                    isVar = true;
                }
            }

            TypeSyntax csType = isVar ? SyntaxFactory.IdentifierName("var") : node.Type.ToCsSyntaxTree<TypeSyntax>();
            return SyntaxFactory
                .VariableDeclaration(csType)
                .AddVariables(node.Declarations.ToCsSyntaxTrees<VariableDeclaratorSyntax>());
        }
    }
}

