namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LessThanEqualsToken)]
    public class LessThanEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.LessThanEqualsToken; }
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
