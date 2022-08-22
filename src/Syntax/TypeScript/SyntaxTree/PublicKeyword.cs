namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PublicKeyword)]
    public class PublicKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PublicKeyword; }
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

        protected override string GetText()
        {
            return "public";
        }
    }
}
