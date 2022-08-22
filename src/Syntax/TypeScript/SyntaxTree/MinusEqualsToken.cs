namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.MinusEqualsToken)]
    public class MinusEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.MinusEqualsToken; }
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
