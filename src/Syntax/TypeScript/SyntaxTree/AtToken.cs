namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AtToken)]
    public class AtToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.AtToken; }
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
