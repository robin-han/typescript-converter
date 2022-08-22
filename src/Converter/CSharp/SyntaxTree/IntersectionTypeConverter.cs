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
    public class IntersectionTypeConverter : NodeConverter
    {
        public List<CSharpSyntaxNode> Convert(IntersectionType node)
        {
            var ret = new List<CSharpSyntaxNode>();
            foreach (var type in node.Types)
            {
                ret.Add(type.ToCsSyntaxTree<CSharpSyntaxNode>());
            }
            return ret;
        }
    }
}

