namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ColonToken)]
    public class ColonToken : Node
    {
        #region Properties        
        public override NodeKind Kind
        {
            get { return NodeKind.ColonToken; }
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
