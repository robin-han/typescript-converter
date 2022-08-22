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
    public class TupleTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(TupleType node) // let a: [number, string] = [1, "hello"];
        {
            GenericNameSyntax generic = this.Context.TypeScriptType
                ? SyntaxFactory.GenericName("Array")
                : SyntaxFactory.GenericName("List");

            Node elementType = this.GetElementType(node);
            if (elementType != null)
            {
                generic = generic.AddTypeArgumentListArguments(elementType.ToCsSyntaxTree<TypeSyntax>());
            }
            else
            {
                generic = this.Context.TypeScriptType
                    ? generic.AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("Object"))
                    : generic.AddTypeArgumentListArguments(SyntaxFactory.IdentifierName("object"));
            }

            return generic;
        }

        private Node GetElementType(TupleType node)
        {
            string type = null;
            foreach (Node item in node.ElementTypes)
            {
                string elementType = TypeHelper.ToShortName(item.Text);
                if (type == null)
                {
                    type = elementType;
                }
                else if (type != elementType)
                {
                    return null;
                }
            }
            return type == null ? null : node.ElementTypes[0];
        }
    }

}