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
    public class FunctionDeclarationConverter : Converter
    {
        public CSharpSyntaxNode Convert(FunctionDeclaration node)
        {
            if (node.Parent != null && node.Parent.Kind == NodeKind.ModuleBlock)
            {
                return this.CreateFunctionDeclaration(node);
            }
            return this.CreateFunctionStatement(node);
        }

        private MethodDeclarationSyntax CreateFunctionDeclaration(FunctionDeclaration node)
        {
            MethodDeclarationSyntax methodDeclaration = SyntaxFactory.MethodDeclaration(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text);

            methodDeclaration = methodDeclaration.AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());
            methodDeclaration = methodDeclaration.AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                methodDeclaration = methodDeclaration.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                methodDeclaration = methodDeclaration.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            return methodDeclaration.WithBody(node.Body.ToCsNode<BlockSyntax>());
        }

        private LocalFunctionStatementSyntax CreateFunctionStatement(FunctionDeclaration node)
        {
            LocalFunctionStatementSyntax funStatement = SyntaxFactory.LocalFunctionStatement(node.Type.ToCsNode<TypeSyntax>(), node.Name.Text);

            funStatement = funStatement.AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());
            funStatement = funStatement.AddParameterListParameters(node.Parameters.ToCsNodes<ParameterSyntax>());

            if (node.JsDoc.Count > 0)
            {
                funStatement = funStatement.WithLeadingTrivia(SyntaxFactory.Trivia(node.JsDoc[0].ToCsNode<DocumentationCommentTriviaSyntax>()));
            }
            if (node.TypeParameters.Count > 0)
            {
                funStatement = funStatement.AddTypeParameterListParameters(node.TypeParameters.ToCsNodes<TypeParameterSyntax>());
            }

            return funStatement.WithBody(node.Body.ToCsNode<BlockSyntax>());
        }

    }
}

