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
    public class QualifiedNameConverter : Converter
    {
        public CSharpSyntaxNode Convert(QualifiedName node)
        {
            SimpleNameSyntax csRight = null;
            List<Node> typeArguments = node.Parent == null ? null : node.Parent.GetValue("TypeArguments") as List<Node>;
            if (typeArguments != null && typeArguments.Count > 0)
            {
                csRight = SyntaxFactory
                    .GenericName(node.Right.Text)
                    .AddTypeArgumentListArguments(typeArguments.ToCsNodes<TypeSyntax>());
            }
            else
            {
                csRight = node.Right.ToCsNode<SimpleNameSyntax>();
            }

            // omit names(dv. core.)
            List<string> omittedNames = this.Context.Config.OmittedQualifiedNames;
            if (omittedNames.Count > 0 && omittedNames.Contains(node.Left.Text.Trim()))
            {
                return csRight;
            }
            // 
            return SyntaxFactory.QualifiedName(node.Left.ToCsNode<NameSyntax>(), csRight);
        }
    }
}

