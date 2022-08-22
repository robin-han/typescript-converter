namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PercentToken)]
    public class PercentToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.PercentToken; }
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
