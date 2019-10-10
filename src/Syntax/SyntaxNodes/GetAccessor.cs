using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class GetAccessor : Node
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.GetAccessor; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> Parameters
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

        public Node Type
        {
            get
            {
                return this._type;
            }
            internal set
            {
                this._type = value;
                if (this._type != null)
                {
                    this._type.Parent = this;
                }
            }
        }

        public Node Body
        {
            get;
            private set;
        }

        public bool IsStatic
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.StaticKeyword);
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.PrivateKeyword);
            }
        }

        public bool IsAbstract
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.AbstractKeyword);
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Modifiers = new List<Node>();
            this.Parameters = new List<Node>();
            this.JsDoc = new List<Node>();

            this.Name = null;
            this.Body = null;
            this.Type = null;
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
                    this.Parameters.Add(childNode);
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "body":
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

