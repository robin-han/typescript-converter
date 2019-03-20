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
    public class TypeLiteralConverter : Converter
    {
        public CSharpSyntaxNode Convert(TypeLiteral node)
        {
            ConverterContext context = LangConverter.CurrentContext;

            List<Node> members = node.Members;

            if (members.Count == 0)
            {
                //TODO: NOT SUPPORT
                return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
            }
            else if (members.Count == 1)
            {
                Node member = members[0];
                switch (member.Kind)
                {
                    case NodeKind.PropertySignature:
                        var csType = (member as PropertySignature).Type.ToCsNode<TypeSyntax>();
                        if (context.PreferTypeScriptType)
                        {
                            return SyntaxFactory.GenericName("Hashtable").AddTypeArgumentListArguments(
                                SyntaxFactory.IdentifierName("String"),
                                csType);
                        }
                        else
                        {
                            return SyntaxFactory.GenericName("Dictionary").AddTypeArgumentListArguments(
                                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                csType);
                        }

                    case NodeKind.IndexSignature:
                        return member.ToCsNode<TypeSyntax>();

                    default:
                        //TODO: NOT SUPPORT
                        return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
                }
            }
            else
            {
                List<TupleElementSyntax> csTupleElements = new List<TupleElementSyntax>();
                foreach (PropertySignature member in members)
                {
                    csTupleElements.Add(SyntaxFactory.TupleElement(member.Type.ToCsNode<TypeSyntax>(), SyntaxFactory.Identifier(member.Name.Text)));
                }

                return SyntaxFactory.TupleType().WithElements(SyntaxFactory.SeparatedList(csTupleElements));
            }
        }
    }
}

