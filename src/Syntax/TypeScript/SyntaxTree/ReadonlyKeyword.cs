namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ReadonlyKeyword)]
    public class ReadonlyKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ReadonlyKeyword; }
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
