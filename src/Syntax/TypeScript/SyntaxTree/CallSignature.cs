using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CallSignature)]
    public class CallSignature : Node
    {
        public CallSignature()
        {
            this.Parameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CallSignature; }
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
