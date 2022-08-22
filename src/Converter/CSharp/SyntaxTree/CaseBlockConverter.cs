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
    public class CaseBlockConverter : NodeConverter
    {
        public SyntaxList<SwitchSectionSyntax> Convert(CaseBlock node)
        {
            return SyntaxFactory.List(node.Clauses.ToCsSyntaxTrees<SwitchSectionSyntax>());
        }
    }
}

