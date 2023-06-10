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
    public class NoSubstitutionTemplateLiteralConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(NoSubstitutionTemplateLiteral node)
        {
            return ConvertNode(node);
        }

        public static CSharpSyntaxNode ConvertNode(Node node)
        {
            SyntaxList<InterpolatedStringContentSyntax> contents = new SyntaxList<InterpolatedStringContentSyntax>();
            string text = node.Text;
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("{", "{{").Replace("}", "}}").Replace("\"", "\\\"");
                SyntaxToken textToken = SyntaxFactory.Token(SyntaxTriviaList.Empty, SyntaxKind.InterpolatedStringTextToken, text, text, SyntaxTriviaList.Empty);
                contents = contents.Add(SyntaxFactory.InterpolatedStringText(textToken));
                return SyntaxFactory.InterpolatedStringExpression(SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken), contents);
            }
            return null;
        }

    }
}
