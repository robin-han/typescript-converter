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
    public class VariableDeclarationConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(VariableDeclaration node)
        {
            //TODO: name is ArrayBindingPattern
            VariableDeclaratorSyntax csVariable = SyntaxFactory.VariableDeclarator(node.Name.Text);

            if (node.Initializer != null)
            {
                csVariable = csVariable.WithInitializer(SyntaxFactory.EqualsValueClause(node.Initializer.ToCsSyntaxTree<ExpressionSyntax>()));
            }

            return csVariable;
        }
    }
}

