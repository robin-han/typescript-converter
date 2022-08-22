namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TrueKeyword)]
    public class TrueKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TrueKeyword; }
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
