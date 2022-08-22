namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ContinueStatement)]
    public class ContinueStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ContinueStatement; }
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
