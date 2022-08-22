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
    public class TemplateSpanConverter : NodeConverter
    {
        public SyntaxList<InterpolatedStringContentSyntax> Convert(TemplateSpan node)
        {
            SyntaxList<InterpolatedStringContentSyntax> contentList = new SyntaxList<InterpolatedStringContentSyntax>();
            contentList = contentList.Add(SyntaxFactory.Interpolation(node.Expression.ToCsSyntaxTree<ExpressionSyntax>()));
            InterpolatedStringTextSyntax literalContent = node.Literal.ToCsSyntaxTree<InterpolatedStringTextSyntax>();
            if (literalContent != null)
            {
                contentList = contentList.Add(literalContent);
            }

            return contentList;
        }
    }
}
