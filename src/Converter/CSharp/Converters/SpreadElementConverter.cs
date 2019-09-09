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
    public class SpreadElementConverter : Converter
    {
        public CSharpSyntaxNode Convert(SpreadElement node)
        {
            if (node.Parent != null && node.Parent.Kind == NodeKind.CallExpression)
            {
                return node.Expression.ToCsNode<ExpressionSyntax>();
            }

            //TODO: NOT SUPPORT
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }
    }
}

