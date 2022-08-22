namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeAssertionExpression)]
    public class TypeAssertionExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeAssertionExpression; }
        }

        public Node Type
        {
            get;
            private set;
        }

        public Node Expression
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

                case "expression":
                    this.Expression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
