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
    public class UnionTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(UnionType node)
        {
            List<Node> types = node.Types;
            if (types.Count == 2)
            {
                if (types[0].Kind == NodeKind.NullKeyword)
                {
                    return types[1].ToCsNode<TypeSyntax>();
                }
                else if (types[1].Kind == NodeKind.NullKeyword)
                {
                    return types[0].ToCsNode<TypeSyntax>();
                }
            }
            return SyntaxFactory.IdentifierName("dynamic");
        }
    }
}

