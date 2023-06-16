namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsEqualsToken)]
    public class EqualsEqualsToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsEqualsToken; }
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
