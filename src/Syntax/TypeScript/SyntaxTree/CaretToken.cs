namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CaretToken)]
    public class CaretToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CaretToken; }
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
