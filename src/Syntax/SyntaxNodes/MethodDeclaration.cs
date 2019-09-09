using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class MethodDeclaration : Declaration
    {
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
            internal set;
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

        public Block Body
        {
            get;
            private set;
        }

        public bool IsStatic
        {
            get
            {
                return this.HasModify(NodeKind.StaticKeyword);
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.HasModify(NodeKind.PrivateKeyword);
            }
        }

        public bool IsAbstract
        {
            get
            {
                return this.HasModify(NodeKind.AbstractKeyword);
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
    }
}

