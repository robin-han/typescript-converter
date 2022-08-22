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
    public class MethodDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(MethodDeclaration node)
        {
            List<Node> modifiers = node.Modifiers;
            if (this.Context.TypeScriptType && node.Name.Text == "toString" && !node.HasModify(NodeKind.OverrideKeyword))
            {
                modifiers = new List<Node>(modifiers);
                modifiers.Add(NodeHelper.CreateNode(NodeKind.OverrideKeyword));
            }

            MethodDeclarationSyntax csMethod = SyntaxFactory
                .MethodDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .AddModifiers(modifiers.ToCsSyntaxTrees<SyntaxToken>())
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

            if (node.Body == null)
            {
                csMethod = csMethod.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                csMethod = csMethod.WithBody(node.Body.ToCsSyntaxTree<BlockSyntax>());
            }

            return csMethod;
        }

    }
}

