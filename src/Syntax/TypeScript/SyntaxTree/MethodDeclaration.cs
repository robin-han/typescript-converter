using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.MethodDeclaration)]
    public class MethodDeclaration : Node
    {
        public MethodDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
            this.Parameters = new List<Node>();
        }

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

        public bool IsGenericType
        {
            get
            {
                string typeText = this.Type.Text;
                if (this.TypeParameters.Find(n => n.Text == typeText) != null)
                {
                    return true;
                }

                ClassDeclaration cls = this.Parent as ClassDeclaration;
                if (cls.TypeParameters.Find(n => n.Text == typeText) != null)
                {
                    return true;
                }

                return false;
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

        public void SetType(Node type, bool changeParent = true)
        {
            this.Type = type;
            if (changeParent && this.Type != null)
            {
                this.Type.Parent = this;
            }
        }

        public void SetName(Node name, bool changeParent = true)
        {
            this.Name = name;
            if (changeParent && this.Name != null)
            {
                this.Name.Parent = this;
            }
        }
    }
}
