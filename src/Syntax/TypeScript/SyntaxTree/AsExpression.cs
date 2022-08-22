namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.AsExpression)]
    public class AsExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.AsExpression; }
        }

        public Node Expression
        {
            get;
            private set;
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
                case "expression":
                    this.Expression = childNode;
                    break;

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
