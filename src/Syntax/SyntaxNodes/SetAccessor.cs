using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class SetAccessor : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SetAccessor; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Parameter> Parameters
        {
            get;
            private set;
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Modifiers = new List<Node>();
            this.Parameters = new List<Parameter>();
            this.JsDoc = new List<Node>();
            this.Name = null;
            this.Body = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "parameters":
                    this.Parameters.Add(childNode as Parameter);
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "body":
                    this.Body = (Block)childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

