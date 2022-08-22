namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PrivateKeyword)]
    public class PrivateKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PrivateKeyword; }
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
