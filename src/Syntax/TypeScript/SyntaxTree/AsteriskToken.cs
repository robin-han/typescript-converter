namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AsteriskToken)]
    public class AsteriskToken : Node
    {        
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AsteriskToken; }
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
