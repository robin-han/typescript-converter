namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.MinusToken)]
    public class MinusToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.MinusToken; }
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
