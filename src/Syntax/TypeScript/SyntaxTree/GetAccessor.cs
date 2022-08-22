using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GetAccessor)]
    public class GetAccessor : Node
    {
        public GetAccessor()
        {
            this.Modifiers = new List<Node>();
            this.Parameters = new List<Node>();
            this.JsDoc = new List<Node>();
            this.Decorators = new List<Node>();
        }

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

        public List<Node> Decorators
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

        public Block Body
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

                case "decorators":
                    this.Decorators.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "body":
                    this.Body = (Block)childNode;
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
