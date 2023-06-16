namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SemicolonClassElement)]
    public class SemicolonClassElement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SemicolonClassElement; }
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
