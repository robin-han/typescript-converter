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
    public class TypeAliasDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TypeAliasDeclaration node)
        {
            if (node.IsDelegate)
            {
                return CreateDelegateDeclaration(node);
            }
            var name = SyntaxFactory.NameEquals(NormalizeTypeName(node.Name));
            if (node.TypeParameters.Count > 0)
            {
                return SyntaxFactory.UsingDirective(name, SyntaxFactory.IdentifierName("dynamic"));
            }
            return SyntaxFactory.UsingDirective(name, node.Type.ToCsSyntaxTree<TypeSyntax>());
        }

        private DelegateDeclarationSyntax CreateDelegateDeclaration(TypeAliasDeclaration node)
        {
            FunctionType fn = node.Type as FunctionType;

            DelegateDeclarationSyntax csDelegateDeclaration = SyntaxFactory
                .DelegateDeclaration(fn.Type.ToCsSyntaxTree<TypeSyntax>(), NormalizeTypeName(node.Name))
                .AddParameterListParameters(fn.Parameters.ToCsSyntaxTrees<ParameterSyntax>())
                .AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>()); ;

            if (node.TypeParameters.Count > 0)
            {
                csDelegateDeclaration = csDelegateDeclaration.AddTypeParameterListParameters(node.TypeParameters.ToCsSyntaxTrees<TypeParameterSyntax>());
            }
            return csDelegateDeclaration;
        }
    }
}
