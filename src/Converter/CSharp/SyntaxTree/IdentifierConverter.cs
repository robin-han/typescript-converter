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
    public class IdentifierConverter : NodeConverter
    {
        private static readonly Dictionary<string, string> IdentifierMappings = new Dictionary<string, string>()
        {
            { "as", "@as" },
            //{ "length", "Length" },
        };

        public CSharpSyntaxNode Convert(Identifier node)
        {
            string text = node.Text;
            if (IdentifierMappings.ContainsKey(text))
            {
                text = IdentifierMappings[text];
            }

            if (text == "length" && node.Parent.Kind == NodeKind.PropertyAccessExpression)
            {
                PropertyAccessExpression parent = node.Parent as PropertyAccessExpression;
                Node type = TypeHelper.GetNodeType(parent.Expression);
                if (type != null)
                {
                    type = TypeHelper.TrimType(type);
                    if (TypeHelper.IsStringType(type))
                    {
                        text = "Length";
                    }
                    else if (TypeHelper.IsArrayType(type))
                    {
                        if (type.Parent != null && type.Parent.Kind == NodeKind.Parameter && (type.Parent as Parameter).IsVariable)
                        {
                            text = "Length";
                        }
                        else
                        {
                            text = "Count";
                        }
                    }
                }
            }

            NameSyntax csNameSyntax = null;
            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            List<Node> arguments = node.Parent == null ? null : node.Parent.GetValue("Arguments") as List<Node>;

            if (typeArguments != null && typeArguments.Count > 0 && (arguments == null || !arguments.Contains(node))) //not in arguments 
            {
                csNameSyntax = SyntaxFactory
                   .GenericName(text)
                   .AddTypeArgumentListArguments(typeArguments.ToCsSyntaxTrees<TypeSyntax>());
            }
            else
            {
                csNameSyntax = SyntaxFactory.IdentifierName(text);
            }

            //
            return this.As(csNameSyntax, node.As);
        }
    }
}

