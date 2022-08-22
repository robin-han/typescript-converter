namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BarBarToken)]
    public class BarBarToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.BarBarToken; }
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
