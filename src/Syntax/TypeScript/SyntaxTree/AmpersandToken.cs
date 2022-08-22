namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AmpersandToken)]
    public class AmpersandToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.AmpersandToken; }
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
