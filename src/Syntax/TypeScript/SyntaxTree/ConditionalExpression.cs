namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ConditionalExpression)]
    public class ConditionalExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ConditionalExpression; }
        }

        public Node Condition
        {
            get;
            private set;
        }

        public Node QuestionToken
        {
            get;
            private set;
        }

        public Node WhenTrue
        {
            get;
            private set;
        }

        public Node ColonToken
        {
            get;
            private set;
        }

        public Node WhenFalse
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
                case "condition":
                    this.Condition = childNode;
                    break;
                case "questionToken":
                    this.QuestionToken = childNode;
                    break;
                case "whenTrue":
                    this.WhenTrue = childNode;
                    break;
                case "colonToken":
                    this.ColonToken = childNode;
                    break;
                case "whenFalse":
                    this.WhenFalse = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
