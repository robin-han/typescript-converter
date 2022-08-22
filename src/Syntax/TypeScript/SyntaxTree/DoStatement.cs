namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.DoStatement)]
    public class DoStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.DoStatement; }
        }

        public Node Statement
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
                case "statement":
                    this.Statement = childNode;
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
