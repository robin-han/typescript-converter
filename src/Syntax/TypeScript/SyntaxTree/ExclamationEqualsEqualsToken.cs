namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExclamationEqualsEqualsToken)]
    public class ExclamationEqualsEqualsToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExclamationEqualsEqualsToken; }
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
