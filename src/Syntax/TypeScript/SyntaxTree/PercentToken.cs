namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PercentToken)]
    public class PercentToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PercentToken; }
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
