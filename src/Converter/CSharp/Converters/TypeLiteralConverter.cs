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
            List<Node> members = node.Members;

            if (members.Count == 0)
            {
                return null;
            }

            switch (members[0].Kind)
            {
                //
                case NodeKind.PropertySignature:
                    if (members.Count == 1)
                    {
                        PropertySignature member = members[0] as PropertySignature;
                        return SyntaxFactory.GenericName("Hashtable").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                            member.Type.ToCsNode<TypeSyntax>());
                    }

                    List<TupleElementSyntax> csTupleElements = new List<TupleElementSyntax>();
                    foreach (PropertySignature member in members)
                    {
                        csTupleElements.Add(SyntaxFactory.TupleElement(member.Type.ToCsNode<TypeSyntax>(), SyntaxFactory.Identifier(member.Name.Text)));
                    }

                    return SyntaxFactory.TupleType().WithElements(SyntaxFactory.SeparatedList(csTupleElements));

                //
                case NodeKind.IndexSignature:
                    return members[0].ToCsNode<TypeSyntax>();

                default:
                    return null;
            }

        }
    }
}

