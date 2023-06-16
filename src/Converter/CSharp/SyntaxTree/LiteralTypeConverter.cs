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
    public class LiteralTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(LiteralType node)
        {
            //TODO: implement custom types instead of using the base type.
            switch (node.Literal.Kind)
            {
                case NodeKind.StringLiteral:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword))
                                    .WithTrailingTrivia(SyntaxFactory.Comment("/*" + node.Literal.Text + "*/"));
                case NodeKind.NumericLiteral:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword))
                                    .WithTrailingTrivia(SyntaxFactory.Comment("/*" + node.Literal.Text + "*/"));
                case NodeKind.TrueKeyword:
                case NodeKind.FalseKeyword:
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword))
                                    .WithTrailingTrivia(SyntaxFactory.Comment("/*" + node.Literal.Text + "*/"));
            }

            //TODO: NOT SUPPORT
            return SyntaxFactory.IdentifierName("dynamic")
                .WithTrailingTrivia(SyntaxFactory.Comment("/*" + node.Literal.Text + "*/"));
        }
    }
}
