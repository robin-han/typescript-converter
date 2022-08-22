namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.IntKeyword)]
    public class IntKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IntKeyword; }
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