namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BarToken)]
    public class BarToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.BarToken; }
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
