namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ProtectedKeyword)]
    public class ProtectedKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ProtectedKeyword; }
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
