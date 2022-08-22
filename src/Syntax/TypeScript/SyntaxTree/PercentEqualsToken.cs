namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PercentEqualsToken)]
    public class PercentEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.PercentEqualsToken; }
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
