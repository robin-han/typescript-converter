namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.NullKeyword)]
    public class NullKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NullKeyword; }
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
