using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.OptionalType)]
    public class OptionalType : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.OptionalType; }
        }

        public Node Type
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Type = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
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
