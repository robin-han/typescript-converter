using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ArrowFunction)]
    public class ArrowFunction : Node
    {
        public ArrowFunction()
        {
            this.Parameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrowFunction; }
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            private set;
        }

        public Node EqualsGreaterThanToken
        {
            get;
            private set;
        }

        public Node Body
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
                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "equalsGreaterThanToken":
                    this.EqualsGreaterThanToken = childNode;
                    break;

                case "body":
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
