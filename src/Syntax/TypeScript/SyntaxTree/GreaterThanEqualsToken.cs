namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GreaterThanEqualsToken)]
    public class GreaterThanEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.GreaterThanEqualsToken; }
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
