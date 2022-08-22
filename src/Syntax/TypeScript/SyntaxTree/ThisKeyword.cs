namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ThisKeyword)]
    public class ThisKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ThisKeyword; }
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
            return "this";
        }
    }
}
