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
    public class GetAccessorConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(GetAccessor node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory
                .PropertyDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text)
                .AddModifiers(node.Modifiers.ToCsSyntaxTrees<SyntaxToken>())
                .AddAttributeLists(node.Decorators.ToCsSyntaxTrees<AttributeListSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            AccessorDeclarationSyntax csGetAccess = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration);
            if (node.Body != null)
            {
                csGetAccess = csGetAccess.WithBody(node.Body.ToCsSyntaxTree<BlockSyntax>());
            }
            else
            {
                csGetAccess = csGetAccess.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }

            return csProperty.AddAccessorListAccessors(csGetAccess);
        }
    }
}

