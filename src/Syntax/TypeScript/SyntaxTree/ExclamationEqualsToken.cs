namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExclamationEqualsToken)]
    public class ExclamationEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.ExclamationEqualsToken; }
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
