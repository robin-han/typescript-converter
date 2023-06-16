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
    public class TypeParameterConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TypeParameter node)
        {
            return SyntaxFactory.TypeParameter(NormalizeTypeName(node.Name));
        }
    }
}
