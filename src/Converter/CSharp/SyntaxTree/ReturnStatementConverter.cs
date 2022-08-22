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
    public class ReturnStatementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ReturnStatement node)
        {
            if (node.Expression == null)
            {
                return SyntaxFactory.ReturnStatement();
            }

            // return default(T); for generic return type
            if (node.Expression.Kind == NodeKind.NullKeyword)
            {
                MethodDeclaration method = node.Ancestor<MethodDeclaration>();
                if (method != null && method.IsGenericType)
                {
                    return SyntaxFactory.ReturnStatement(SyntaxFactory.DefaultExpression(SyntaxFactory.IdentifierName(method.Type.Text)));
                }
            }

            return SyntaxFactory.ReturnStatement(node.Expression.ToCsSyntaxTree<ExpressionSyntax>());
        }
    }
}

