using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class JSDocCommentConverter : JSDocBaseConverter
    {
        public CSharpSyntaxNode Convert(JSDocComment node)
        {
            SyntaxList<XmlNodeSyntax> comments = SyntaxFactory.List(this.CreateXmlTextBlock("summary", this.GetComment(node)));

            if (node.Tags.Count > 0)
            {
                foreach (Node tag in node.Tags)
                {
                    if (!this.IsDocCommentTag(tag))
                    {
                        comments = comments.AddRange(tag.ToCsNode<SyntaxList<XmlNodeSyntax>>());
                    }
                }
            }
            comments = comments.Add(this.CreateXmlTextNewLine());

            return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, comments);
        }

        private string GetComment(JSDocComment node)
        {
            string comment = node.Comment;

            foreach (Node tag in node.Tags)
            {
                if (this.IsDocCommentTag(tag))
                {
                    comment += (tag as JSDocTag).Text;
                }
            }

            return comment;
        }

        private bool IsDocCommentTag(Node tag)
        {
            if (tag.Kind != NodeKind.JSDocTag)
            {
                return false;
            }

            JSDocTag docTag = tag as JSDocTag;

            return (docTag.TagName.Text != "example");
        }
    }
}

