using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class ConstructorNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.Constructor:
                    this.NormalizeConstructor(node as Constructor);
                    break;

                default:
                    break;
            }

        }

        private void NormalizeConstructor(Constructor ctorNode)
        {
            List<Node> statements = (ctorNode.Body as Block).Statements;
            for (int i = 0; i < statements.Count; i++)
            {
                Node statement = statements[i];
                if (this.IsBaseConstructor(statement))
                {
                    ctorNode.Body.Remove(statement);
                    ctorNode.Base = ctorNode.CreateNode(statement.TsNode);
                    break;
                }
            }
        }

        private bool IsBaseConstructor(Node node)
        {
            if (node.Kind != NodeKind.ExpressionStatement)
            {
                return false;
            }

            ExpressionStatement expStatement = node as ExpressionStatement;
            if (expStatement.Expression.Kind != NodeKind.CallExpression)
            {
                return false;
            }

            CallExpression callExp = expStatement.Expression as CallExpression;
            if (callExp.Expression.Kind != NodeKind.SuperKeyword)
            {
                return false;
            }

            return true;
        }

    }
}
