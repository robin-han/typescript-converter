namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PlusEqualsToken)]
    public class PlusEqualsToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PlusEqualsToken; }
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
