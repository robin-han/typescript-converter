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
    public class EnumDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(EnumDeclaration node)
        {
            EnumDeclarationSyntax csEnum = SyntaxFactory.EnumDeclaration(node.Name.Text);

            csEnum = csEnum.AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());
            csEnum = csEnum.AddMembers(node.Members.ToCsNodes<EnumMemberDeclarationSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csEnum = csEnum.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }

            return csEnum;
        }
    }
}

