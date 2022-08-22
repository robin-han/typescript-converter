using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;
using System;

namespace TypeScript.Converter.CSharp
{
    public class JSDocBaseConverter : NodeConverter
    {
        protected XmlNodeSyntax[] CreateXmlTextBlock(string tag, string text, List<KeyValuePair<string, string>> attrs = null)
        {
            SyntaxList<XmlAttributeSyntax> attrList = SyntaxFactory.List<XmlAttributeSyntax>();
            if (attrs != null && attrs.Count > 0)
            {
                foreach (var attr in attrs)
                {
                    attrList = attrList.Add(SyntaxFactory.XmlNameAttribute(
                        SyntaxFactory.XmlName(attr.Key),
                        SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
                        attr.Value,
                        SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken)));
                }
            }

            return new XmlNodeSyntax[] {
                SyntaxFactory.XmlText(this.CreateXmlTextLiteral()),
                SyntaxFactory.XmlElement(
                    SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(tag), attrList),
                    SyntaxFactory.SingletonList<XmlNodeSyntax>(SyntaxFactory.XmlText(SyntaxFactory
                        .TokenList(this.SeparateText(text))
                        .Add(this.CreateXmlTextLiteral())
                    )),
                    SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(tag))),
            };
        }

        protected XmlNodeSyntax[] CreateXmlTextBlock(string tag, XmlNodeSyntax[] xmlNodes)
        {
            return new XmlNodeSyntax[] {
                SyntaxFactory.XmlText(this.CreateXmlTextLiteral()),
                SyntaxFactory.XmlElement(
                    SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(tag)),
                    SyntaxFactory.List<XmlNodeSyntax>(xmlNodes).Add(SyntaxFactory.XmlText(this.CreateXmlTextLiteral())),
                    SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(tag)))
             };
        }

        protected SyntaxToken CreateXmlTextLiteral(string text = "")
        {
            SyntaxTriviaList leadingTrivial = SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///"));

            return SyntaxFactory.XmlTextLiteral(
                leadingTrivial,
                " " + text,
                "",
                SyntaxFactory.TriviaList());
        }

        protected XmlTextSyntax CreateXmlTextNewLine()
        {
            return SyntaxFactory.XmlText(SyntaxFactory.XmlTextNewLine(
                SyntaxFactory.TriviaList(),
                Environment.NewLine,
                "",
                SyntaxFactory.TriviaList()));
        }

        protected List<SyntaxToken> SeparateText(string text)
        {
            List<SyntaxToken> syntaxTokens = new List<SyntaxToken>();

            string[] texts = text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            for (int i = 0; i < texts.Length; i++)
            {
                string txt = texts[i];
                if (string.IsNullOrEmpty(txt) && (i == 0 || string.IsNullOrEmpty(texts[i - 1])))
                {
                    continue;
                }
                syntaxTokens.Add(this.CreateXmlTextLiteral(txt));
            }

            return syntaxTokens;
        }
    }
}
