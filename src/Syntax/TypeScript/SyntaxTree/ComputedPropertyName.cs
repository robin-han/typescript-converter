using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ComputedPropertyName)]
    public class ComputedPropertyName : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ComputedPropertyName; }
        }

        public Node Expression
        {
            get;
            private set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
