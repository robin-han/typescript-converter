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
    public class ParameterConverter : Converter
    {
        public CSharpSyntaxNode Convert(Parameter node)
        {
            ParameterSyntax csParameter = SyntaxFactory
                .Parameter(SyntaxFactory.Identifier(node.Name.Text))
                .WithType(node.Type.ToCsNode<TypeSyntax>());

            if (node.Initializer != null)
            {
                csParameter = csParameter.WithDefault(SyntaxFactory.EqualsValueClause(node.Initializer.ToCsNode<ExpressionSyntax>()));
            }

            if (node.DotDotDotToken != null)
            {
                csParameter = csParameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.ParamsKeyword));
            }

            return csParameter;
        }
    }

}