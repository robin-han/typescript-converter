namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.FalseKeyword)]
    public class FalseKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.FalseKeyword; }
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
