namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsEqualsEqualsToken)]
    public class EqualsEqualsEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsEqualsEqualsToken; }
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
