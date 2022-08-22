namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SlashEqualsToken)]
    public class SlashEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.SlashEqualsToken; }
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
