using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PropertyDeclaration)]
    public class PropertyDeclaration : Node
    {
        public PropertyDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Decorators = new List<Node>();
        }

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

        public List<Node> Decorators
        {
            get;
            private set;
        }

        public virtual Node Name
        {
            get;
            private set;
        }

        public virtual Node Type
        {
            get;
            private set;
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node QuestionToken
        {
            get;
            private set;
        }

        public bool IsPublic
        {
            get
            {
                return !IsProtected && !IsPrivate;
            }
        }

        public bool IsProtected
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ProtectedKeyword);
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

        #endregion

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

                case "decorators":
                    this.Decorators.Add(childNode);
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

                case "questionToken":
                    this.QuestionToken = childNode;
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
