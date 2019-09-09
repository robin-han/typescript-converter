using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class MethodSignature : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.MethodSignature; }
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

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public List<Node> Parameters
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
            this.Name = null;
            this.TypeParameters = new List<Node>();
            this.Parameters = new List<Node>();
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

                case "name":
                    this.Name = childNode;
                    break;

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "parameters":
                    this.Parameters.Add(childNode);
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

