namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExportKeyword)]
    public class ExportKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExportKeyword; }
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
