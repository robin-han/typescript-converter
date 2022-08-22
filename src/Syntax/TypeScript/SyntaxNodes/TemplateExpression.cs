using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    public class TemplateExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TemplateExpression; }
        }

        public Node Head
        {
            get;
            private set;
        }

        public List<Node> TemplateSpans
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Head = null;
            this.TemplateSpans = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "head":
                    this.Head = childNode;
                    break;

                case "templateSpans":
                    this.TemplateSpans.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
