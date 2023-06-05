namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AtToken)]
    public class AtToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AtToken; }
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
