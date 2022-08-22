namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AnyKeyword)]
    public class AnyKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AnyKeyword; }
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
