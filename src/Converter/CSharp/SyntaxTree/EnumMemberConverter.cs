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
    public class EnumMemberConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(EnumMember node)
        {
            EnumMemberDeclarationSyntax csEnumMember = SyntaxFactory.EnumMemberDeclaration(node.Name.Text);

            if (node.Initializer != null)
            {
                csEnumMember = csEnumMember.WithEqualsValue(SyntaxFactory.EqualsValueClause(node.Initializer.ToCsSyntaxTree<ExpressionSyntax>()));
            }

            if (node.JsDoc.Count > 0)
            {
                csEnumMember = csEnumMember.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsSyntaxTree<DocumentationCommentTriviaSyntax>()));
            }

            return csEnumMember;
        }
    }
}

