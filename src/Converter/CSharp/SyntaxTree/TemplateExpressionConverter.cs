using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class TemplateExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TemplateExpression node)
        {
            SyntaxList<InterpolatedStringContentSyntax> contents = new SyntaxList<InterpolatedStringContentSyntax>();
            InterpolatedStringContentSyntax head = node.Head.ToCsSyntaxTree<InterpolatedStringContentSyntax>();
            if (head != null)
            {
                contents = contents.Add(head);
            }

            foreach (Node templateSpan in node.TemplateSpans)
            {
                var content = templateSpan.ToCsSyntaxTree<SyntaxList<InterpolatedStringContentSyntax>>();
                contents = contents.AddRange(content);
            }

            return SyntaxFactory.InterpolatedStringExpression(SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken), contents);
        }
    }
}

