namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SlashToken)]
    public class SlashToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.SlashToken; }
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
