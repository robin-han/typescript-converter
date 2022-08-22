namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CommaToken)]
    public class CommaToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.CommaToken; }
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
