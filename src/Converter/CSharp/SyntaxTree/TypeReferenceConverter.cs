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
    public class TypeReferenceConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TypeReference node)
        {
            string typeText = node.TypeName.Text;

            switch (typeText)
            {
                case NativeTypes.Int:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));

                case NativeTypes.Long:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword));

                case NativeTypes.AnyObject:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));

                case NativeTypes.Bool:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));

                case NativeTypes.Double:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));

                case NativeTypes.String:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));

                case "Array":
                case "ReadonlyArray":
                    if (node.TypeArguments.Count > 0)
                    {
                        var typeArgs = GetValidTypeArguments(node);
                        if (this.Context.TypeScriptType)
                        {
                            return SyntaxFactory
                                .GenericName("Array")
                                .AddTypeArgumentListArguments(typeArgs[0].ToCsSyntaxTree<TypeSyntax>());
                        }
                        else
                        {
                            return SyntaxFactory
                                .GenericName("List")
                                .AddTypeArgumentListArguments(typeArgs[0].ToCsSyntaxTree<TypeSyntax>());
                        }
                    }
                    else
                    {
                        return node.TypeName.ToCsSyntaxTree<TypeSyntax>();
                    }

                case "RegExpMatchArray":
                case "RegExpExecArray":
                    return SyntaxFactory.IdentifierName("RegExpArray");

                default:
                    if (node.TypeArguments.Count > 0)
                    {
                        return SyntaxFactory
                            .GenericName(TypeHelper.ToShortName(typeText))
                            .AddTypeArgumentListArguments(GetValidTypeArguments(node).ToCsSyntaxTrees<TypeSyntax>());
                    }
                    else
                    {
                        return node.TypeName.ToCsSyntaxTree<TypeSyntax>();
                    }
            }
        }

        private static List<Node> GetValidTypeArguments(TypeReference node)
        {
            var ret = new List<Node>();
            foreach (var type in node.TypeArguments)
            {
                if (type.Kind == NodeKind.VoidKeyword)
                    continue;
                ret.Add(type);
            }
            return ret;
        }
    }
}
