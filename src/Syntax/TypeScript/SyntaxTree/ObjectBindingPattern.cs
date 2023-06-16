using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ObjectBindingPattern)]
    public class ObjectBindingPattern : Node
    {

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ObjectBindingPattern; }
        }

        public List<Node> Elements
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Elements = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "elements":
                    this.Elements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

