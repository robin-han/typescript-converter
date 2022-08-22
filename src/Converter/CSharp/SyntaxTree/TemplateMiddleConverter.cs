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
    public class TemplateMiddleConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TemplateMiddle node)
        {
            string text = node.Text;
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("{", "{{").Replace("}", "}}").Replace("\"", "\\\"");
                SyntaxToken textToken = SyntaxFactory.Token(SyntaxTriviaList.Empty, SyntaxKind.InterpolatedStringTextToken, text, text, SyntaxTriviaList.Empty);
                return SyntaxFactory.InterpolatedStringText(textToken);
            }
            return null;
        }
    }
}
