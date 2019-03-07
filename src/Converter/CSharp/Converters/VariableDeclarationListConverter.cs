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
            Node type = node.Type;
            if ((type.Kind == NodeKind.AnyKeyword || type.Kind == NodeKind.ObjectKeyword) && node.Declarations.Count > 0)
            {
                VariableDeclarationNode variableNode = node.Declarations[0] as VariableDeclarationNode;
                if (variableNode.Initializer != null)
                {
                    type = node.CreateNode(NodeKind.Identifier, "var");
                }
            }

            return SyntaxFactory
                .VariableDeclaration(type.ToCsNode<TypeSyntax>())
                .AddVariables(node.Declarations.ToCsNodes<VariableDeclaratorSyntax>());
        }
    }
}

