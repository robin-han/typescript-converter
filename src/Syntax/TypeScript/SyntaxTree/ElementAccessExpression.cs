using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ElementAccessExpression)]
    public class ElementAccessExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ElementAccessExpression; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node ArgumentExpression
        {
            get;
            private set;
        }

        public List<string> Parts
        {
            get
            {
                List<string> parts = new List<string>();
                if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                {
                    parts.InsertRange(0, ((PropertyAccessExpression)this.Expression).Parts);
                }
                else if (this.Expression.Kind == NodeKind.CallExpression)
                {
                    parts.InsertRange(0, ((CallExpression)this.Expression).Parts);
                }
                else
                {
                    parts.Insert(0, this.Expression.Text);
                }
                parts[parts.Count - 1] = parts[parts.Count - 1] + $"[{this.ArgumentExpression.Text}]";
                return parts;
            }
        }
        #endregion


        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                case "argumentExpression":
                    this.ArgumentExpression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
