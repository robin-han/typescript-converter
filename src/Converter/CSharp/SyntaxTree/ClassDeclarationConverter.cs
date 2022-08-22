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
    public class ClassDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ClassDeclaration node)
        {
            ClassDeclarationSyntax csClass = SyntaxFactory
                .ClassDeclaration(node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>())
                .AddMembers(node.Members.ToCsSyntaxTrees<MemberDeclarationSyntax>())
                .AddAttributeLists(node.Decorators.ToCsSyntaxTrees<AttributeListSyntax>());


            foreach (TypeParameter typeParameter in node.TypeParameters)
            {
                csClass = csClass.AddTypeParameterListParameters(typeParameter.ToCsSyntaxTree<TypeParameterSyntax>());
                if (typeParameter.Constraint != null)
                {
                    var constrains = new List<TypeParameterConstraintSyntax>();
                    foreach (var type in (typeParameter.Constraint.ToCsSyntaxTrees<TypeSyntax>()))
                    {
                        constrains.Add(SyntaxFactory.TypeConstraint(type));
                    }

                    csClass = csClass.AddConstraintClauses(SyntaxFactory
                        .TypeParameterConstraintClause(typeParameter.Name.Text)
                        .AddConstraints(constrains.ToArray())
                    );
                }
            }

            if (node.Extending != null)
            {
                csClass = csClass.AddBaseListTypes(node.Extending.ToCsSyntaxTree<BaseTypeSyntax>());
            }
            else if (this.Context.TypeScriptType)
            {
                csClass = csClass.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName("Object")));
            }
            if (node.Implementing.Count > 0)
            {
                csClass = csClass.AddBaseListTypes(node.Implementing.ToCsSyntaxTrees<BaseTypeSyntax>());
            }

            if (node.JsDoc.Count > 0)
            {
                csClass = csClass.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            return csClass;
        }
    }
}

