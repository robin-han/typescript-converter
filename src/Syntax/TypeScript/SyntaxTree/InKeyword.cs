namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.InKeyword)]
    public class InKeyword : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.InKeyword; }
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
