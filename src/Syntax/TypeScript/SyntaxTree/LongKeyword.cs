namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LongKeyword)]
    public class LongKeyword : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.LongKeyword; }
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
