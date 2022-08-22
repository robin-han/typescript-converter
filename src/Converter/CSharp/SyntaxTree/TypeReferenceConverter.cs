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
                //case "String":
                //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));

                //case "Number":
                //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));

                //case "Boolean":
                //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));

                //case "Object":
                //    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));

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
                    if (this.Context.TypeScriptType)
                    {
                        return SyntaxFactory
                            .GenericName("Array")
                            .AddTypeArgumentListArguments(node.TypeArguments[0].ToCsSyntaxTree<TypeSyntax>());
                    }
                    else
                    {
                        return SyntaxFactory
                            .GenericName("List")
                            .AddTypeArgumentListArguments(node.TypeArguments[0].ToCsSyntaxTree<TypeSyntax>());
                    }

                case "RegExpMatchArray":
                case "RegExpExecArray":
                    return SyntaxFactory.IdentifierName("RegExpArray");

                default:
                    if (node.TypeArguments.Count > 0)
                    {
                        return SyntaxFactory
                            .GenericName(TypeHelper.ToShortName(typeText))
                            .AddTypeArgumentListArguments(node.TypeArguments.ToCsSyntaxTrees<TypeSyntax>());
                    }
                    else
                    {
                        return node.TypeName.ToCsSyntaxTree<TypeSyntax>();
                    }
            }
        }

    }
}

