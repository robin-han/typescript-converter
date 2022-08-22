namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GreaterThanGreaterThanGreaterThanToken)]
    public class GreaterThanGreaterThanGreaterThanToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.GreaterThanGreaterThanGreaterThanToken; }
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
