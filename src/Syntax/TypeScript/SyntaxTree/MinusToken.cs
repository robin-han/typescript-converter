namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.MinusToken)]
    public class MinusToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.MinusToken; }
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
