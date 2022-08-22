using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.FunctionExpression)]
    public class FunctionExpression : Node
    {
        public FunctionExpression()
        {
            this.Parameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.FunctionExpression; }
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            private set;
        }

        private int ModifierFlagsCache { get; set; }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "body":
                    this.Body = (Block)childNode;
                    break;

                default:
                    // this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
