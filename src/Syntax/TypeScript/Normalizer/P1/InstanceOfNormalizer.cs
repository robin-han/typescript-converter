using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class InstanceOfNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 10; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.InstanceOfKeyword:
                    this.NormalizeInstanceOf(node.Parent as BinaryExpression);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeInstanceOf(BinaryExpression instanceOf)
        {
            Node leftNode = instanceOf.Left;
            Node leftNodeType = TypeHelper.GetNodeType(leftNode);
            if (leftNodeType != null && TypeHelper.ToShortName(leftNodeType.Text) == "DataValueType")
            {
                PropertyAccessExpression newLeft = NodeHelper.CreateNode(NodeKind.PropertyAccessExpression) as PropertyAccessExpression;
                newLeft.Parent = leftNode.Parent;
                leftNode.Parent = newLeft;

                newLeft.SetExpression(leftNode);
                newLeft.SetName(NodeHelper.CreateNode(NodeKind.Identifier, "Value"));
                instanceOf.SetLeft(newLeft);
                return;
            }

            NodeKind variableKind = instanceOf.Left.Kind;
            string variableName = instanceOf.Left.Text;
            string typeName = instanceOf.Right.Text;
            List<Node> asNodes = new List<Node>();
            Node parent = instanceOf.Parent;
            while (parent != null)
            {
                if (parent.Kind == NodeKind.Block || parent.Kind == NodeKind.CaseBlock)
                {
                    break;
                }
                else if (parent.Kind == NodeKind.IfStatement)
                {
                    List<Node> multiInstanceOf = (parent as IfStatement).Expression.DescendantsAndSelf(n =>
                    {
                        if (n.Kind == NodeKind.BinaryExpression && n != instanceOf)
                        {
                            BinaryExpression binary = n as BinaryExpression;
                            return (binary.OperatorToken.Kind == NodeKind.InstanceOfKeyword && binary.Left.Text == variableName);
                        }
                        return false;
                    });
                    if (multiInstanceOf.Count == 0)
                    {
                        asNodes.AddRange((parent as IfStatement).ThenStatement.Descendants(n => n.Kind == variableKind && n.Text == variableName));
                    }
                    break;
                }
                else if (parent.Kind == NodeKind.BinaryExpression)
                {
                    BinaryExpression binaryExpr = parent as BinaryExpression;
                    bool instanceOfIsLeftNode = (binaryExpr.Left.DescendantsAndSelfOnce(n => n == instanceOf).Count > 0);
                    if (instanceOfIsLeftNode)
                    {
                        asNodes.AddRange(binaryExpr.Right.Descendants(n =>
                        {
                            bool isInInstanceOf = (n.Parent.Kind == NodeKind.BinaryExpression && (n.Parent as BinaryExpression).OperatorToken.Kind == NodeKind.InstanceOfKeyword);
                            return (n.Kind == variableKind && n.Text == variableName && !isInInstanceOf);
                        }));
                    }
                }
                parent = parent.Parent;
            }

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
                        if (!(asNode.Parent.Kind == NodeKind.BinaryExpression && (asNode.Parent as BinaryExpression).Left == asNode))
                        {
                            (asNode as PropertyAccessExpression).As = typeName;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

