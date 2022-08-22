namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.DotDotDotToken)]
    public class DotDotDotToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.DotDotDotToken; }
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
