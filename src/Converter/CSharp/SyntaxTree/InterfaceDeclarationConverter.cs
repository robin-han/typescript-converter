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
    public class InterfaceDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(InterfaceDeclaration node)
        {
            if (node.IsDelegate)
            {
                return this.CreateDelegateDeclaration(node);
            }

            InterfaceDeclarationSyntax csInterface = SyntaxFactory
                .InterfaceDeclaration(node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>())
                .AddMembers(node.Members.ToCsSyntaxTrees<MemberDeclarationSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csInterface = csInterface.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                foreach (TypeParameter typeParameter in node.TypeParameters)
                {
                    csInterface = csInterface.AddTypeParameterListParameters(typeParameter.ToCsSyntaxTree<TypeParameterSyntax>());
                    if (typeParameter.Constraint != null)
                    {
                        var constrains = new List<TypeParameterConstraintSyntax>();
                        foreach (var type in (typeParameter.Constraint.ToCsSyntaxTrees<TypeSyntax>()))
                        {
                            constrains.Add(SyntaxFactory.TypeConstraint(type));
                        }

                        csInterface = csInterface.AddConstraintClauses(SyntaxFactory
                            .TypeParameterConstraintClause(typeParameter.Name.Text)
                            .AddConstraints(constrains.ToArray())
                        );
                    }
                }
            }
            if (node.BaseTypes.Count > 0)
            {
                csInterface = csInterface.AddBaseListTypes(node.BaseTypes.ToCsSyntaxTrees<BaseTypeSyntax>());
            }

            return csInterface;
        }

        private DelegateDeclarationSyntax CreateDelegateDeclaration(InterfaceDeclaration node)
        {
            CallSignature callSignature = node.Members[0] as CallSignature;

            DelegateDeclarationSyntax csDelegateDeclaration = SyntaxFactory
                .DelegateDeclaration(callSignature.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .AddParameterListParameters(callSignature.Parameters.ToCsSyntaxTrees<ParameterSyntax>());

            if (node.TypeParameters.Count > 0)
            {
                csDelegateDeclaration = csDelegateDeclaration.AddTypeParameterListParameters(node.TypeParameters.ToCsSyntaxTrees<TypeParameterSyntax>());
            }

            return csDelegateDeclaration;
        }
    }
}

