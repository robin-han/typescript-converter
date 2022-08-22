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
    public class MethodSignatureConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(MethodSignature node)
        {
            MethodDeclarationSyntax csMethod = SyntaxFactory
                .MethodDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .AddParameterListParameters(node.Parameters.ToCsSyntaxTrees<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csMethod = csMethod.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            if (node.TypeParameters.Count > 0)
            {
                foreach (TypeParameter typeParameter in node.TypeParameters)
                {
                    csMethod = csMethod.AddTypeParameterListParameters(typeParameter.ToCsSyntaxTree<TypeParameterSyntax>());
                    if (typeParameter.Constraint != null)
                    {
                        var constrains = new List<TypeParameterConstraintSyntax>();
                        foreach (var type in (typeParameter.Constraint.ToCsSyntaxTrees<TypeSyntax>()))
                        {
                            constrains.Add(SyntaxFactory.TypeConstraint(type));
                        }

                        csMethod = csMethod.AddConstraintClauses(SyntaxFactory
                            .TypeParameterConstraintClause(typeParameter.Name.Text)
                            .AddConstraints(constrains.ToArray())
                        );
                    }
                }
            }

            return csMethod.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
    }

}

