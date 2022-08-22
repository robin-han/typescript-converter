namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsEqualsToken)]
    public class EqualsEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsEqualsToken; }
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
