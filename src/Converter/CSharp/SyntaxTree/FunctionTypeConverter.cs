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
    public class FunctionTypeConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(FunctionType node)
        {
            var parameters = node.Parameters.ToCsSyntaxTrees<ParameterSyntax>().Select(p => p.Type).ToList();

            string delegateName;
            if ((node.Type.Kind == NodeKind.VoidKeyword))
            {
                delegateName = "Action";
            }
            else
            {
                delegateName = "Func";
                parameters.Add(node.Type.ToCsSyntaxTree<TypeSyntax>());
            }            

            // Create delegate type syntax
            var delegateType = SyntaxFactory.GenericName(delegateName);
            if (parameters.Count > 0)
            {
                delegateType = delegateType.WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(parameters)));
            }

            return delegateType;
        }
    }
}
