namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GreaterThanToken)]
    public class GreaterThanToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.GreaterThanToken; }
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
