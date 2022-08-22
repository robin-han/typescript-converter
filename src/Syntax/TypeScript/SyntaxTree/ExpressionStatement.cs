using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExpressionStatement)]
    public class ExpressionStatement : Node
    {
        public ExpressionStatement()
        {
            this.JsDoc = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExpressionStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public List<Node> JsDoc
        {
            get;
            private set;
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

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void SetExpression(Node expression, bool changeParent = true)
        {
            this.Expression = expression;
            if (changeParent && this.Expression != null)
            {
                this.Expression.Parent = this;
            }
        }
    }
}
