namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.StaticKeyword)]
    public class StaticKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.StaticKeyword; }
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
