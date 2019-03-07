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
    public class IdentifierConverter : Converter
    {
        public CSharpSyntaxNode Convert(Identifier node)
        {
            NameSyntax csNameSyntax = null;
            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                csNameSyntax = SyntaxFactory
                   .GenericName(node.Text)
                   .AddTypeArgumentListArguments(typeArguments.ToCsNodes<TypeSyntax>());
            }
            else
            {
                csNameSyntax = SyntaxFactory.IdentifierName(node.Text);
            }

            //
            string asType = node.As;
            if (string.IsNullOrEmpty(asType))
            {
                return csNameSyntax;
            }
            else
            {
                asType = this.StripType(asType);
                BinaryExpressionSyntax csAs = SyntaxFactory.BinaryExpression(
                    SyntaxKind.AsExpression,
                    csNameSyntax,
                    SyntaxFactory.ParseName(asType));
                return SyntaxFactory.ParenthesizedExpression(csAs);
            }
        }
    }
}

