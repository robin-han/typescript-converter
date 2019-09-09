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
    public class TypeReferenceConverter : Converter
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

                case "Integer":
                case "IntegerType":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));

                case "LongType":
                case "LongIntegerType":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword));

                case "ObjectType":
                case "AnyObject":
                case "DataValueObject":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));

                case "Array":
                    if (node.IsParams)
                    {
                        return SyntaxFactory
                            .ArrayType(node.TypeArguments[0].ToCsNode<TypeSyntax>())
                            .AddRankSpecifiers(SyntaxFactory.ArrayRankSpecifier());
                    }

                    if (this.Context.Config.PreferTypeScriptType)
                    {
                        return SyntaxFactory
                            .GenericName("Array")
                            .AddTypeArgumentListArguments(node.TypeArguments[0].ToCsNode<TypeSyntax>());
                    }
                    else
                    {
                        return SyntaxFactory
                            .GenericName("List")
                            .AddTypeArgumentListArguments(node.TypeArguments[0].ToCsNode<TypeSyntax>());
                    }

                case "RegExpMatchArray":
                case "RegExpExecArray":
                    //if (this.Context.Config.PreferTypeScriptType)
                    //{
                    //    return SyntaxFactory
                    //        .GenericName("Array")
                    //        .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("String"));
                    //}
                    //else
                    //{
                    //    return SyntaxFactory
                    //        .GenericName("List")
                    //        .AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)));
                    //}
                    return SyntaxFactory.IdentifierName("RegExpArray");

                default:
                    return node.TypeName.ToCsNode<TypeSyntax>();
            }
        }

    }
}

