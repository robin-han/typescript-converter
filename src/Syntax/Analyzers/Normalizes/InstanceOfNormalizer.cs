using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
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
            List<Node> inOfs = ifStatement.Expression.DescendantsAndSelf(n =>
                n.Kind == NodeKind.BinaryExpression &&
                (n as BinaryExpression).OperatorToken.Kind == NodeKind.InstanceOfKeyword);

            List<Node> instanceofs = new List<Node>();
            inOfs.ForEach(n1 =>
            {
                if (inOfs.Find(n2 => (n2 != n1 && (n1 as BinaryExpression).Left.Text.Trim() == (n2 as BinaryExpression).Left.Text.Trim())) == null)
                {
                    instanceofs.Add(n1);
                }
            });

            foreach (BinaryExpression instanceof in instanceofs)
            {
                Node leftNode = instanceof.Left;
                Node leftNodeType = TypeHelper.GetNodeType(leftNode);
                if (leftNodeType != null && TypeHelper.ToShortName(leftNodeType.Text) == "DataValueType")
                {
                    PropertyAccessExpression newLeft = NodeHelper.CreateNode(NodeKind.PropertyAccessExpression) as PropertyAccessExpression;
                    newLeft.Parent = leftNode.Parent;
                    leftNode.Parent = newLeft;

                    newLeft.Expression = leftNode;
                    newLeft.Name = NodeHelper.CreateNode(NodeKind.Identifier, "Value");
                    instanceof.Left = newLeft;
                    continue;
                }

                NodeKind variableKind = instanceof.Left.Kind;
                string variableName = instanceof.Left.Text.Trim();
                string typeName = instanceof.Right.Text;

                List<Node> asNodes = new List<Node>();
                asNodes.AddRange(ifStatement.Expression.DescendantsAndSelf(n => n.Kind == variableKind && n.Text.Trim() == variableName).FindAll(n => n.Parent != instanceof));
                asNodes.AddRange(ifStatement.ThenStatement.Descendants(n => n.Kind == variableKind && n.Text.Trim() == variableName));
                foreach (Node asNode in asNodes)
                {
                    switch (asNode.Kind)
                    {
                        case NodeKind.Identifier:
                            if (asNode.Parent.Kind == NodeKind.PropertyAccessExpression)
                            {
                                if (asNode == (asNode.Parent as PropertyAccessExpression).Expression)
                                {
                                    (asNode as Identifier).As = typeName;
                                }
                            }
                            else
                            {
                                (asNode as Identifier).As = typeName;
                            }
                            break;

                        case NodeKind.PropertyAccessExpression:
                            (asNode as PropertyAccessExpression).As = typeName;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

    }
}

