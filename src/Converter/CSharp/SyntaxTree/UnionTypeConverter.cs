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
    public class UnionTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(UnionType node)
        {
            if (node.Types.Count == 2 && node.HasNullType)
            {
                Node type = TypeHelper.TrimType(node);
                if (this.IsNullableType(type))
                {
                    return SyntaxFactory.NullableType(type.ToCsSyntaxTree<TypeSyntax>());
                }
                else
                {
                    return type.ToCsSyntaxTree<TypeSyntax>();
                }
            }
            return SyntaxFactory.IdentifierName("dynamic");
        }

        private bool IsNullableType(Node type)
        {
            if (type.Kind == NodeKind.TypeLiteral || type.Kind == NodeKind.BooleanKeyword || type.Kind == NodeKind.NumberKeyword)
            {
                return true;
            }

            string typeName = TypeHelper.ToShortName(type.Text);
            if (typeName == NativeTypes.Int || typeName == NativeTypes.Long || typeName == NativeTypes.Double || typeName == NativeTypes.Bool)
            {
                return true;
            }

            Node definition = type.Document.GetTypeDeclaration(typeName);
            if (definition != null && definition.Kind == NodeKind.EnumDeclaration)
            {
                return true;
            }

            return false;
        }
    }
}

