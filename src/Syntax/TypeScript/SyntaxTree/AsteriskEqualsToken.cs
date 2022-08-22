namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AsteriskEqualsToken)]
    public class AsteriskEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.AsteriskEqualsToken; }
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
