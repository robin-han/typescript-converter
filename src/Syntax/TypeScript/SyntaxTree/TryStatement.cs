namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TryStatement)]
    public class TryStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TryStatement; }
        }

        public Node TryBlock
        {
            get;
            private set;
        }

        public Node CatchClause
        {
            get;
            private set;
        }

        public Node FinallyBlock
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
                case "tryBlock":
                    this.TryBlock = childNode;
                    break;

                case "catchClause":
                    this.CatchClause = childNode;
                    break;

                case "finallyBlock":
                    this.FinallyBlock = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
