namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CatchClause)]
    public class CatchClause : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CatchClause; }
        }

        public VariableDeclaration VariableDeclaration
        {
            get;
            private set;
        }

        public Node Block
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
                case "variableDeclaration":
                    this.VariableDeclaration = childNode as VariableDeclaration;
                    break;

                case "block":
                    this.Block = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
