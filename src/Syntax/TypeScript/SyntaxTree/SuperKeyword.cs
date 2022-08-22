namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SuperKeyword)]
    public class SuperKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SuperKeyword; }
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
            return "super";
        }
    }
}
