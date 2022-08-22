namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.WhileStatement)]
    public class WhileStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.WhileStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
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

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
