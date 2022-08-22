namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.StringKeyword)]
    public class StringKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.StringKeyword; }
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
