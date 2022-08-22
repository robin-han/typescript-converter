namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LiteralType)]
    public class LiteralType : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.LiteralType; }
        }

        public Node Literal
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
                case "literal":
                    this.Literal = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
