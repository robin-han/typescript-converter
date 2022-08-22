namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EmptyStatement)]
    public class EmptyStatement : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.EmptyStatement; }
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
