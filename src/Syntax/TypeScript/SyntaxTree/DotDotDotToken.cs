namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.DotDotDotToken)]
    public class DotDotDotToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.DotDotDotToken; }
        }
        #endregion

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
