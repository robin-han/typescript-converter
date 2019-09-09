using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class HeritageClause : Node
    {
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

        public List<Node> Types //ExpressionWithTypeArguments
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Types = new List<Node>();

            JToken jsonToken = jsonObj["token"];
            this.Token = jsonToken == null ? NodeKind.Unknown : (NodeKind)jsonToken.ToObject<int>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

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

