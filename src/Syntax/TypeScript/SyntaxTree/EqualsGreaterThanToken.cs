namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsGreaterThanToken)]
    public class EqualsGreaterThanToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsGreaterThanToken; }
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
