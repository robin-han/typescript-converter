namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocTypeExpression)]
    public class JSDocTypeExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocTypeExpression; }
        }

        public Node Type
        {
            get;
            private set;
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "type":
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
