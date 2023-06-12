namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.FirstTemplateToken)]
    public class FirstTemplateToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.FirstTemplateToken; }
        }
        
        public string RawText
        {
            get;
            private set;
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
