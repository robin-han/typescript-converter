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
    public class JSDocPrivateTagConverter : JSDocBaseConverter
    {
        public SyntaxList<XmlNodeSyntax> Convert(JSDocPrivateTag node)
        {
            return SyntaxFactory.List<XmlNodeSyntax>(new XmlNodeSyntax[] {
                SyntaxFactory.XmlText(this.CreateXmlTextLiteral("private"))});
        }       
    }
}
