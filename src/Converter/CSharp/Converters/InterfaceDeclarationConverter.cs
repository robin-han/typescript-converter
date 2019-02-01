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
    public class InterfaceDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(InterfaceDeclaration node)
        {
            if (this.IsDelegateDeclaration(node))
            {
                return this.CreateDelegateDeclaration(node);
            }

            InterfaceDeclarationSyntax csInterface = SyntaxFactory
                .InterfaceDeclaration(node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>())
                .AddMembers(node.Members.ToCsNodes<MemberDeclarationSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csInterface = csInterface.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                csInterface = csInterface.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }
            if (node.BaseTypes.Count > 0)
            {
                csInterface = csInterface.AddBaseListTypes(node.BaseTypes.ToCsNodes<BaseTypeSyntax>());
            }

            return csInterface;
        }

        private bool IsDelegateDeclaration(InterfaceDeclaration node)
        {
            return (node.Members.Count == 1 && node.Members[0].Kind == NodeKind.CallSignature);
        }

        private DelegateDeclarationSyntax CreateDelegateDeclaration(InterfaceDeclaration node)
        {
            CallSignature callSignature = node.Members[0] as CallSignature;

            DelegateDeclarationSyntax csDelegateDeclaration = SyntaxFactory
                .DelegateDeclaration(callSignature.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddParameterListParameters(callSignature.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.TypeParameters.Count > 0)
            {
                csDelegateDeclaration = csDelegateDeclaration.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            return csDelegateDeclaration;
        }
    }
}

