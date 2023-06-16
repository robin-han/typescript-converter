using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PropertyAssignment)]
    public class PropertyAssignment : Node
    {
        public PropertyAssignment()
        {
            this.JsDoc = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyAssignment; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
