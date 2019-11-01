using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
            if (node.Types.Count == 2 && node.HasNullType)
            {
                Node type = TypeHelper.TrimNullUnionType(node);
                if (this.IsNullableType(type))
                {
                    return SyntaxFactory.NullableType(type.ToCsNode<TypeSyntax>());
                }
                else
                {
                    return type.ToCsNode<TypeSyntax>();
                }
            }
            return SyntaxFactory.IdentifierName("dynamic");
        }

        private bool IsNullableType(Node type)
        {
            if (type.Kind == NodeKind.NumberKeyword && !this.Context.Config.PreferTypeScriptType)
            {
                return true;
            }

            Node definition = type.Document.GetTypeDefinition(TypeHelper.ToShortName(type.Text));
            if (definition != null && definition.Kind == NodeKind.EnumDeclaration)
            {
                return true;
            }

            return false;
        }
    }
}

