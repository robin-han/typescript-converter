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
    public class ParameterConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(Parameter node)
        {
            ParameterSyntax csParameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(node.Name.Text));
            TypeSyntax csType = null;
            if (node.IsVariable)
            {
                csType = SyntaxFactory
                    .ArrayType(node.VariableType.ToCsSyntaxTree<TypeSyntax>())
                    .AddRankSpecifiers(SyntaxFactory.ArrayRankSpecifier());
            }
            else if (node.Type != null)
            {
                csType = node.Type.ToCsSyntaxTree<TypeSyntax>();
            }

            Node initializer = node.Initializer;
            if (node.IsOptional && initializer == null)
            {
                initializer = Parameter.CreateInitializer(node.Type);
            }
            if (initializer != null)
            {
                csParameter = csParameter.WithDefault(SyntaxFactory.EqualsValueClause(initializer.ToCsSyntaxTree<ExpressionSyntax>()));

                // A(alias: number = 1): void
                if (initializer.Kind != NodeKind.NullKeyword && initializer.Kind != NodeKind.UndefinedKeyword)
                {
                    switch (node.Type.Kind)
                    {
                        case NodeKind.NumberKeyword:
                            csType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));
                            break;

                        case NodeKind.StringKeyword:
                            csType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
                            break;

                        case NodeKind.BooleanKeyword:
                            csType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));
                            break;

                        default:
                            break;
                    }
                }
            }

            if (node.IsVariable)
            {
                csParameter = csParameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.ParamsKeyword));
            }
            if (csType != null && !ShouldIgnoreType(node))
            {
                csParameter = csParameter.WithType(csType);
            }

            return csParameter;
        }

        private bool ShouldIgnoreType(Parameter node)
        {
            if (node.Parent.Kind == NodeKind.ArrowFunction)
            {
                return true;
            }

            if (node.Parent.Kind == NodeKind.FunctionExpression && node.Parent.Parent.Kind == NodeKind.CallExpression)
            {
                return true;
            }
            return false;
        }

    }
}