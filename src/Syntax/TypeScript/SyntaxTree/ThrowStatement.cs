namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ThrowStatement)]
    public class ThrowStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ThrowStatement; }
        }

        public Node Expression
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
