namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EqualsGreaterThanToken)]
    public class EqualsGreaterThanToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EqualsGreaterThanToken; }
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
