namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.NumericLiteral)]
    public class NumericLiteral : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NumericLiteral; }
        }

        #region Ignored Properties
        private int NumericLiteralFlags { get; set; }
        #endregion
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
