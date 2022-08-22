namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.InstanceOfKeyword)]
    public class InstanceOfKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.InstanceOfKeyword; }
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
