using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class CompleteNodeNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ReturnStatement:
                    this.CompleteReturnStatement(node as ReturnStatement);
                    break;

                default:
                    break;
            }
        }

        private void CompleteReturnStatement(ReturnStatement returnStatement)
        {
            if (returnStatement.Expression != null)
            {
                return;
            }

            MethodDeclaration method = returnStatement.Ancestor(NodeKind.MethodDeclaration) as MethodDeclaration;
            if (method != null && method.Type != null && method.Type.Kind == NodeKind.UnionType && ((UnionType)method.Type).HasNullType)
            {
                returnStatement.Expression = NodeHelper.CreateNode(NodeKind.NullKeyword);
            }
        }
    }
}
