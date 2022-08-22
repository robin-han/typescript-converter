using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.HeritageClause)]
    public class HeritageClause : Node
    {
        public HeritageClause()
        {
            this.Types = new List<Node>();
            this.Token = NodeKind.Unknown;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.HeritageClause; }
        }

        public NodeKind Token
        {
            get;
            private set;
        }

        public List<Node> Types
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            JToken jsonToken = jsonObj["token"];
            this.Token = jsonToken == null ? NodeKind.Unknown : (NodeKind)jsonToken.ToObject<int>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "types":
                    this.Types.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
