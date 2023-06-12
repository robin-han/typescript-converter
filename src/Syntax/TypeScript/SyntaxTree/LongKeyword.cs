namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LongKeyword)]
    public class LongKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.LongKeyword; }
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
