namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GreaterThanToken)]
    public class GreaterThanToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.GreaterThanToken; }
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
