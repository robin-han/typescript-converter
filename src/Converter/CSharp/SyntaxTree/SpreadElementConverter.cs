using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class SpreadElementConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(SpreadElement node)
        {
            if (node.Parent.Kind == NodeKind.CallExpression)
            {
                if (ShouldConvertToArray(node))
                {
                    return SyntaxFactory.InvocationExpression(
                       SyntaxFactory.MemberAccessExpression(
                           SyntaxKind.SimpleMemberAccessExpression,
                           node.Expression.ToCsSyntaxTree<ExpressionSyntax>(),
                           SyntaxFactory.IdentifierName("ToArray")))
                       .AddArgumentListArguments();
                }
                else
                {
                    return node.Expression.ToCsSyntaxTree<ExpressionSyntax>();
                }
            }

            //TODO: NOT SUPPORT
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }

        /// <summary>
        /// Change to array for variable parameter.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldConvertToArray(SpreadElement node)
        {
            if (node.Expression.Kind == NodeKind.Identifier)
            {
                string name = node.Expression.Text;

                // method
                MethodDeclaration methodParent = node.Ancestor<MethodDeclaration>();
                List<Node> paramters = methodParent == null ? new List<Node>() : methodParent.Parameters;
                foreach (Parameter param in paramters)
                {
                    if (param.Name.Text == name && param.IsVariable)
                    {
                        return false;
                    }
                }

                // constructor
                Constructor ctorParent = node.Ancestor<Constructor>();
                paramters = ctorParent == null ? new List<Node>() : ctorParent.Parameters;
                foreach (Parameter param in paramters)
                {
                    if (param.Name.Text == name && param.IsVariable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

