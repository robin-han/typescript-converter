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
    public class JSDocTagConverter : JSDocBaseConverter
    {
        public SyntaxList<XmlNodeSyntax> Convert(JSDocTag node)
        {
            string tag = node.TagName.Text;
            switch (tag)
            {
                case "example":
                    XmlNodeSyntax[] code = this.CreateXmlTextBlock("code", node.Comment);
                    return SyntaxFactory.List<XmlNodeSyntax>(this.CreateXmlTextBlock("example", code));
                default:
                    return SyntaxFactory.List<XmlNodeSyntax>(this.CreateXmlTextBlock("summary", node.Comment));
            }
        }

    }
}

