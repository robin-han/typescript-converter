namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BarEqualsToken)]
    public class BarEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.BarEqualsToken; }
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
