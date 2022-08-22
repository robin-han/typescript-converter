namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsToken)]
    public class EqualsToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsToken; }
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
