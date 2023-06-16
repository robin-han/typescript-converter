using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ShorthandPropertyAssignment)]
    public class ShorthandPropertyAssignment : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ShorthandPropertyAssignment; }
        }

        public Node Name
        {
            get;
            private set;
        }
        
        public Node Initializer
        {
            get;
            protected set;
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
