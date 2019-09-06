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
    public class JSDocParameterTagConverter : JSDocBaseConverter
    {
        public SyntaxList<XmlNodeSyntax> Convert(JSDocParameterTag node)
        {
            return SyntaxFactory.List(this.CreateXmlTextBlock(
                "param",
                node.Comment.TrimStart(' ', '-'),
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("name", node.Name.Text)
                }));
        }

    }
}

