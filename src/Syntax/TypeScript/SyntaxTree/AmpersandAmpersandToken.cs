namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AmpersandAmpersandToken)]
    public class AmpersandAmpersandToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.AmpersandAmpersandToken; }
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
