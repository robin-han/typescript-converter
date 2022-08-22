namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.VoidKeyword)]
    public class VoidKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VoidKeyword; }
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
