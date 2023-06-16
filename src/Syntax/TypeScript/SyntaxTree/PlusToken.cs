namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PlusToken)]
    public class PlusToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PlusToken; }
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
