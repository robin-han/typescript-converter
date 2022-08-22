namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SemicolonClassElement)]
    public class SemicolonClassElement : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.SemicolonClassElement; }
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
