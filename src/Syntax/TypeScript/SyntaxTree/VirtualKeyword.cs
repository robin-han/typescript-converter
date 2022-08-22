namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.VirtualKeyword)]
    public class VirtualKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VirtualKeyword; }
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
            return "virtual";
        }
    }
}
