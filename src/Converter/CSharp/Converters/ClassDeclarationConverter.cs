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
    public class ClassDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(ClassDeclaration node)
        {
            ClassDeclarationSyntax csClass = SyntaxFactory
                .ClassDeclaration(node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>())
                .AddMembers(node.Members.ToCsNodes<MemberDeclarationSyntax>());


            foreach (TypeParameter typeParameter in node.TypeParameters)
            {
                csClass = csClass.AddTypeParameterListParameters(typeParameter.ToCsNode<TypeParameterSyntax>());
                if (typeParameter.Constraint != null)
                {
                    csClass = csClass.AddConstraintClauses(SyntaxFactory
                        .TypeParameterConstraintClause(typeParameter.Name.Text)
                        .AddConstraints(SyntaxFactory.TypeConstraint(typeParameter.Constraint.ToCsNode<TypeSyntax>()))
                    );
                }
            }

            List<Node> baseTypes = node.BaseTypes;
            if (baseTypes.Count > 0)
            {
                csClass = csClass.AddBaseListTypes(baseTypes.ToCsNodes<BaseTypeSyntax>());
            }
            if (node.JsDoc.Count > 0)
            {
                csClass = csClass.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            return csClass;
        }
    }
}

