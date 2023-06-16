namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BreakStatement)]
    public class BreakStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.BreakStatement; }
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
