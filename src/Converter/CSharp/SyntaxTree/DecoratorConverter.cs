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
    public class DecoratorConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(Decorator node)
        {
            // @log
            if (node.Expression.Kind == NodeKind.Identifier)
            {
                return SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList<AttributeSyntax>().Add(
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName(TypeHelper.GetTypeName(node.Expression)))
                ));
            }

            // @log(xxx, xxx)
            if (node.Expression.Kind == NodeKind.CallExpression)
            {
                CallExpression callExpr = (CallExpression)node.Expression;

                var attrArgList = SyntaxFactory.SeparatedList<AttributeArgumentSyntax>();
                foreach (var argument in callExpr.Arguments)
                {
                    if (argument.Kind == NodeKind.Identifier)
                    {
                        attrArgList = attrArgList.Add(SyntaxFactory.AttributeArgument(SyntaxFactory.TypeOfExpression(argument.ToCsSyntaxTree<TypeSyntax>())));
                    }
                    else
                    {
                        attrArgList = attrArgList.Add(SyntaxFactory.AttributeArgument(argument.ToCsSyntaxTree<ExpressionSyntax>()));
                    }
                }

                return SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList<AttributeSyntax>().Add(
                   SyntaxFactory.Attribute(
                       SyntaxFactory.ParseName(TypeHelper.GetTypeName(callExpr.Expression)),
                       SyntaxFactory.AttributeArgumentList(attrArgList)
                   )
                ));
            }

            return null;
        }
    }


}
