namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AmpersandAmpersandToken)]
    public class AmpersandAmpersandToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AmpersandAmpersandToken; }
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
