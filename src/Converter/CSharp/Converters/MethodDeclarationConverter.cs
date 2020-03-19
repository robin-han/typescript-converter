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
    public class MethodDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(MethodDeclaration node)
        {
            List<Node> modifiers = node.Modifiers;
            if (this.Context.Config.PreferTypeScriptType && node.Name.Text == "toString" && !node.HasModify(NodeKind.OverrideKeyword))
            {
                modifiers = new List<Node>(modifiers);
                modifiers.Add(NodeHelper.CreateNode(NodeKind.OverrideKeyword));
            }

            MethodDeclarationSyntax csMethod = SyntaxFactory
                .MethodDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text)
                .AddModifiers(modifiers.ToCsNodes<SyntaxToken>())
                .AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                csMethod = csMethod.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                csMethod = csMethod.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            if (node.Body == null)
            {
                csMethod = csMethod.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                csMethod = csMethod.WithBody(node.Body.ToCsNode<BlockSyntax>());
            }

            return csMethod;
        }

    }
}

