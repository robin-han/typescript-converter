namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BinaryExpression)]
    public class BinaryExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.BinaryExpression; }
        }

        public Node Left
        {
            get;
            private set;
        }

        public Node OperatorToken
        {
            get;
            private set;
        }

        public Node Right
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
                case "left":
                    this.Left = childNode;
                    break;

                case "operatorToken":
                    this.OperatorToken = childNode;
                    break;

                case "right":
                    this.Right = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void SetLeft(Node left, bool changeParent = true)
        {
            this.Left = left;
            if (changeParent && this.Left != null)
            {
                this.Left.Parent = this;
            }
        }

        public void SetOperatorToken(Node operatorToken, bool changeParent = true)
        {
            this.OperatorToken = operatorToken;
            if (changeParent && this.OperatorToken != null)
            {
                this.OperatorToken.Parent = this;
            }
        }

        public void SetRight(Node right, bool changeParent = true)
        {
            this.Right = right;
            if (changeParent && this.Right != null)
            {
                this.Right.Parent = this;
            }
        }
    }
}
