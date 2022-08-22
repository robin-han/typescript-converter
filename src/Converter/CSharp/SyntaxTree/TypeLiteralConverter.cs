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
    public class TypeLiteralConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TypeLiteral node)
        {
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
                        var csType = (member as PropertySignature).Type.ToCsSyntaxTree<TypeSyntax>();
                        if (this.Context.TypeScriptType)
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
                        return member.ToCsSyntaxTree<TypeSyntax>();

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
                    csTupleElements.Add(SyntaxFactory.TupleElement(member.Type.ToCsSyntaxTree<TypeSyntax>(), SyntaxFactory.Identifier(member.Name.Text)));
                }

                return SyntaxFactory.TupleType().WithElements(SyntaxFactory.SeparatedList(csTupleElements));

                //return SyntaxFactory.IdentifierName("dynamic");
            }
        }
    }
}

