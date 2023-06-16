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
                if (IsNullableType(type))
                {
                    return SyntaxFactory.NullableType(type.ToCsSyntaxTree<TypeSyntax>());
                }
                else
                {
                    return type.ToCsSyntaxTree<TypeSyntax>();
                }
            }
            if (!this.Context.TypeScriptAdvancedType)
            {
                return SyntaxFactory.IdentifierName("dynamic");
            }
            // All types are the same, happens on literal types
            var elType = GetMergedElementType(node);
            if (elType != null)
            {
                var synType = elType.ToCsSyntaxTree<TypeSyntax>();
                if (!HasNullElementType(node) || IsNullableType(elType))
                    return synType;
                return SyntaxFactory.GenericName("Nullable")
                        .AddTypeArgumentListArguments(synType);
            }
            // Convert the types
            var retType = SyntaxFactory
                    .GenericName("OrType")
                    .AddTypeArgumentListArguments(GetElementTypes(node).ToArray());
            if (!HasNullElementType(node))
                return retType;
            return SyntaxFactory.GenericName("Nullable")
                    .AddTypeArgumentListArguments(retType);
        }

        private static bool IsNullableType(Node type)
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

        private static bool HasNullElementType(UnionType node)
        {
            foreach (Node item in node.Types)
            {
                if (item.Kind == NodeKind.NullKeyword)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<TypeSyntax> GetElementTypes(UnionType node)
        {
            var ret = new List<TypeSyntax>();
            foreach (var type in node.Types)
            {
                if (type.Kind == NodeKind.NullKeyword)
                    continue;
                ret.Add(type.ToCsSyntaxTree<TypeSyntax>());
            }
            return ret;
        }

        private static Node GetMergedElementType(UnionType node)
        {
            string type = null;
            foreach (var item in GetElementTypes(node))
            {
                string elementType = TypeHelper.ToShortName(item.ToString());
                if (type == null)
                {
                    type = elementType;
                }
                else if (type != elementType)
                {
                    return null;
                }
            }
            return type == null ? null : node.Types[0];
        }
    }
}
