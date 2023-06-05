using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeOperator)]
    public class TypeOperator : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeOperator; }
        }

        public Node Type
        {
            get;
            private set;
        }

        public NodeKind Operator
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Type = null;

            JToken jsonOperator = jsonObj["operator"];
            this.Operator = jsonOperator == null ? NodeKind.Unknown : (NodeKind)jsonOperator.ToObject<int>();
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
