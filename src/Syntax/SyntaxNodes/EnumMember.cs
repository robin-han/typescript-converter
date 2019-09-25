using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class EnumMember : Node
    {
        #region
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

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.Initializer = null;
            this.JsDoc = new List<Node>();
        }

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

