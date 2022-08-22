using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PrefixUnaryExpression)]
    public class PrefixUnaryExpression : Node
    {
        public PrefixUnaryExpression()
        {
            this.Operator = NodeKind.Unknown;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PrefixUnaryExpression; }
        }

        public Node Operand
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

            JToken jsonOperator = jsonObj["operator"];
            this.Operator = jsonOperator == null ? NodeKind.Unknown : (NodeKind)jsonOperator.ToObject<int>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "operand":
                    this.Operand = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
