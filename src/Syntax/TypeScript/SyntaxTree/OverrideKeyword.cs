namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.OverrideKeyword)]
    public class OverrideKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.OverrideKeyword; }
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
            return "override";
        }
    }
}
