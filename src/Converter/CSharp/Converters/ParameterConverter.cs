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
    public class ParameterConverter : Converter
    {
        public CSharpSyntaxNode Convert(Parameter node)
        {
            ParameterSyntax csParameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(node.Name.Text));
            TypeSyntax csType = node.Type.ToCsNode<TypeSyntax>();

            Node initializer = node.Initializer;
            if (node.IsOptional && initializer == null)
            {
                initializer = NodeHelper.CreateNode(NodeKind.NullKeyword);
            }
            if (initializer != null)
            {
                csParameter = csParameter.WithDefault(SyntaxFactory.EqualsValueClause(initializer.ToCsNode<ExpressionSyntax>()));

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
            if (!node.IgnoreType)
            {
                csParameter = csParameter.WithType(csType);
            }

            return csParameter;
        }
    }

}