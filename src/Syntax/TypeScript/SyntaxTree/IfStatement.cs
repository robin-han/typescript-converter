namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.IfStatement)]
    public class IfStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IfStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node ThenStatement
        {
            get;
            private set;
        }

        public Node ElseStatement
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
                case "thenStatement":
                    this.ThenStatement = childNode;
                    break;
                case "elseStatement":
                    this.ElseStatement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
