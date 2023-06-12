namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ImportKeyword)]
    public class ImportKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ImportKeyword; }
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
