using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
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

                default:
                    return node.TypeName.ToCsNode<TypeSyntax>();
            }
        }

    }
}

