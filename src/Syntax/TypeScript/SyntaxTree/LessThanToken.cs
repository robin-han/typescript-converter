namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.LessThanToken)]
    public class LessThanToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.LessThanToken; }
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
