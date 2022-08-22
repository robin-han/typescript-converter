namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.StringLiteral)]
    public class StringLiteral : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.StringLiteral; }
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
