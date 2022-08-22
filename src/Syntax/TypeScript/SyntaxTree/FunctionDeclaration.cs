using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.FunctionDeclaration)]
    public class FunctionDeclaration : Node
    {
        public FunctionDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
            this.Parameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.FunctionDeclaration; }
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

        public virtual Node Name
        {
            get;
            private set;
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public virtual List<Node> Parameters
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }

        public virtual Node Type
        {
            get;
            private set;
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
