namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LessThanLessThanToken)]
    public class LessThanLessThanToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.LessThanLessThanToken; }
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
