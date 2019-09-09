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
    public class PropertyDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(PropertyDeclaration node)
        {
            if (node.IsPublic && !node.IsStatic)
            {
                return this.ToGetSet(node);
            }

            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());

            if (node.Initializer != null)
            {
                csProperty = csProperty.WithInitializer(SyntaxFactory.EqualsValueClause(node.Initializer.ToCsNode<ExpressionSyntax>()));
            }
            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            return csProperty;
        }

        private CSharpSyntaxNode ToGetSet(PropertyDeclaration node)
        {
            List<Node> modifiers = node.Modifiers.FindAll(n => n.Kind != NodeKind.ReadonlyKeyword);

            if (node.IsReadonly)
            {
                PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddModifiers(modifiers.ToCsNodes<SyntaxToken>());

                if (node.JsDoc.Count > 0)
                {
                    csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
                }

                AccessorDeclarationSyntax csGetAccess = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration);
                if (node.Initializer != null)
                {
                    csGetAccess = csGetAccess.WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(node.Initializer.ToCsNode<ExpressionSyntax>())));
                }
                else
                {
                    csGetAccess = csGetAccess.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                }
                return csProperty.AddAccessorListAccessors(csGetAccess);
            }
            else
            {
                PropertyDeclarationSyntax csProperty = SyntaxFactory
                    .PropertyDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                    .AddModifiers(modifiers.ToCsNodes<SyntaxToken>());

                AccessorDeclarationSyntax csGetAccess = SyntaxFactory
                    .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                AccessorDeclarationSyntax csSetAccess = SyntaxFactory
                    .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                if (node.JsDoc.Count > 0)
                {
                    csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
                }
                return csProperty.AddAccessorListAccessors(csGetAccess, csSetAccess);
            }
        }
    }
}

