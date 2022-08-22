namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AsteriskToken)]
    public class AsteriskToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.AsteriskToken; }
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
