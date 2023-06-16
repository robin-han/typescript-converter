namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EmptyStatement)]
    public class EmptyStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EmptyStatement; }
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
