namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EndOfFileToken)]
    public class EndOfFileToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.EndOfFileToken; }
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
