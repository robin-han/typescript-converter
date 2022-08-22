namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ForInStatement)]
    public class ForInStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForInStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
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
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
