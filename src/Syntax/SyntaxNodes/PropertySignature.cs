using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertySignature : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertySignature; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node QuestionToken
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            internal set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Name = null;
            this.QuestionToken = null;
            this.Type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "questionToken":
                    this.QuestionToken = childNode;
                    break;

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

