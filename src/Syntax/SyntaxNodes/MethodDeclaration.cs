using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class MethodDeclaration : Declaration
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.MethodDeclaration; }
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
            get
            {
                if (this._type == null)
                {
                    this._type = this.InferType();
                }
                return this._type;
            }
            private set
            {
                this._type = value;
            }
        }

        public Block Body
        {
            get;
            private set;
        }

        public bool IsStatic
        {
            get
            {
                return this.Modifiers.Exists(m => m.Kind == NodeKind.StaticKeyword);
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
            this.Parameters = new List<Node>();

            this.Name = null;
            this.Type = null;
            this.Body = null;
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

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "body":
                    this.Body = childNode as Block;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            return this.CreateNode(NodeKind.VoidKeyword);
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            if (!this.HasModify(NodeKind.PublicKeyword) && !this.HasModify(NodeKind.PrivateKeyword) && !this.HasModify(NodeKind.ProtectedKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.PublicKeyword));
            }

            if (this.HasJsDocTag("csoverride") && !this.HasModify(NodeKind.OverrideKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.OverrideKeyword));
            }
            if (this.HasJsDocTag("csnew") && !this.HasModify(NodeKind.NewKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.NewKeyword));
            }
        }

        private bool HasModify(NodeKind modify)
        {
            return this.Modifiers.Exists(n => n.Kind == modify);
        }
    }
}

