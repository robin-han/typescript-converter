namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AmpersandToken)]
    public class AmpersandToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AmpersandToken; }
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
