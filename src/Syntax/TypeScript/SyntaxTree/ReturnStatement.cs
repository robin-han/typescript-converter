namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ReturnStatement)]
    public class ReturnStatement : Node
    {
        private Node _expression;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ReturnStatement; }
        }

        public Node Expression
        {
            get
            {
                return this._expression;
            }
            internal set
            {
                this._expression = value;
                if (this._expression != null)
                {
                    this._expression.Parent = this;
                }
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
