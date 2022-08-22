using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EnumMember)]
    public class EnumMember : Node
    {
        public EnumMember()
        {
            this.JsDoc = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EnumMember; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public List<Node> JsDoc
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
                case "name":
                    this.Name = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
