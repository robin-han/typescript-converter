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
    public class PropertyDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(PropertyDeclaration node)
        {
            if (node.IsPublic && !node.IsStatic)
            {
                return this.ToGetSet(node);
            }

            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>())
                .AddAttributeLists(node.Decorators.ToCsSyntaxTrees<AttributeListSyntax>());

            if (node.Initializer != null)
            {
                csProperty = csProperty.WithInitializer(SyntaxFactory.EqualsValueClause(node.Initializer.ToCsSyntaxTree<ExpressionSyntax>()));
            }
            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            return csProperty;
        }

        private CSharpSyntaxNode ToGetSet(PropertyDeclaration node)
        {
            List<Node> modifiers = node.Modifiers.FindAll(n => n.Kind != NodeKind.ReadonlyKeyword);

            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .AddModifiers(modifiers.ToCsSyntaxTrees<SyntaxToken>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            //GetAccess
            AccessorDeclarationSyntax csGetAccess = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            //    if (node.Initializer != null)
            //    {
            //        csGetAccess = csGetAccess.WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(node.Initializer.ToCsNode<ExpressionSyntax>())));
            //    }
            csProperty = csProperty.AddAccessorListAccessors(csGetAccess);

            //SetsAccess
            if ((node.IsReadonly && !node.IsAbstract) || (!node.IsReadonly))
            {
                AccessorDeclarationSyntax csSetAccess = SyntaxFactory
                  .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                  .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                if (node.IsReadonly)
                {
                    csSetAccess = csSetAccess.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                }
                csProperty = csProperty.AddAccessorListAccessors(csSetAccess);
            }

            return csProperty;
        }
    }
}

