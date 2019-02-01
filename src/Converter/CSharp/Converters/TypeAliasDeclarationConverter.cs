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
    public class TypeAliasDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(TypeAliasDeclaration node)
        {
            if (IsDelegateDeclaration(node))
            {
                return CreateDelegateDeclaration(node);
            }
            return SyntaxFactory.UsingDirective(SyntaxFactory.NameEquals(node.Name.Text), node.Type.ToCsNode<NameSyntax>());
        }

        private bool IsDelegateDeclaration(TypeAliasDeclaration node)
        {
            return node.Type.Kind == NodeKind.FunctionType;
        }

        private DelegateDeclarationSyntax CreateDelegateDeclaration(TypeAliasDeclaration node)
        {
            FunctionType fn = node.Type as FunctionType;

            DelegateDeclarationSyntax csDelegateDeclaration = SyntaxFactory
                .DelegateDeclaration(fn.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddParameterListParameters(fn.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.TypeParameters.Count > 0)
            {
                csDelegateDeclaration = csDelegateDeclaration.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }
            return csDelegateDeclaration;
        }
    }
}

