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
    public class PropertySignatureConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(PropertySignature node)
        {
            PropertyDeclarationSyntax csProperty = SyntaxFactory.PropertyDeclaration(node.Type.ToCsSyntaxTree<TypeSyntax>(), node.Name.Text);

            if (node.Parent != null && node.Parent.Kind == NodeKind.InterfaceDeclaration)
            {
                csProperty = csProperty.AddAccessorListAccessors(SyntaxFactory
                    .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                if (!node.IsReadonly)
                {
                    csProperty = csProperty.AddAccessorListAccessors(SyntaxFactory
                        .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
                }
            }
            if (node.JsDoc.Count > 0)
            {
                csProperty = csProperty.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            return csProperty;
        }
    }
}

