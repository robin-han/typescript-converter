using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class InstanceOfNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.IfStatement:
                    this.NormalizeInstanceOf(node as IfStatement);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeInstanceOf(IfStatement ifStatement)
        {
            List<Node> instanceofs = ifStatement.Expression.DescendantsAndSelf(n =>
                n.Kind == NodeKind.BinaryExpression &&
                (n as BinaryExpression).OperatorToken.Kind == NodeKind.InstanceOfKeyword);
            if (instanceofs.Count == 0)
            {
                return;
            }

            BinaryExpression instanceof = instanceofs[0] as BinaryExpression;
            string variableName = instanceof.Left.Text;
            string typeName = instanceof.Right.Text;
            List<Node> asNodes = ifStatement.ThenStatement.Descendants(n => n.Kind == NodeKind.Identifier && n.Text == variableName);
            foreach (Identifier asNode in asNodes)
            {
                if (asNode.Parent.Kind == NodeKind.PropertyAccessExpression)
                {
                    if (asNode == (asNode.Parent as PropertyAccessExpression).Expression)
                    {
                        asNode.As = typeName;
                    }
                }
                else
                {
                    asNode.As = typeName;
                }
            }
        }

    }
}

