using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertyDeclaration : Declaration
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyDeclaration; }
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

        public Node Type
        {
            get;
            private set;
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public bool IsPublic
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.PublicKeyword);
            }
        }

        public bool IsStatic
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.StaticKeyword);
            }
        }

        public bool IsReadonly
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ReadonlyKeyword);
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

        private Node QuestionToken { get; set; }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Name = null;
            this.Type = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

                case "type":
                    this.Type = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void SetType(Node type, bool changeParent = true)
        {
            this.Type = type;
            if (changeParent && this.Type != null)
            {
                this.Type.Parent = this;
            }
        }
    }
}

