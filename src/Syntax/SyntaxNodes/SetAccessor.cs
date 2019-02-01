using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
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

        public Node Body
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

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

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
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            if (!this.Modifiers.Exists(n => n.Kind == NodeKind.PublicKeyword || n.Kind == NodeKind.PrivateKeyword || n.Kind == NodeKind.ProtectedKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.PublicKeyword));
            }

            string paramName = this.Parameters[0].Name.Text;
            if (paramName != "value")
            {
                List<Node> referencedNode = this.Body.Descendants(n => n.Kind == NodeKind.Identifier && n.Text == paramName);
                foreach (Node node in referencedNode)
                {
                    node.Text = "value";
                }
            }
        }

    }
}

