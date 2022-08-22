namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CaretToken)]
    public class CaretToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.CaretToken; }
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
