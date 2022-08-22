using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PropertySignature)]
    public class PropertySignature : Node
    {
        public PropertySignature()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertySignature; }
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

        public Node QuestionToken
        {
            get;
            private set;
        }

        public virtual Node Type
        {
            get;
            private set;
        }

        public bool IsReadonly
        {
            get
            {
                return this.HasModify(NodeKind.ReadonlyKeyword);
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

                case "questionToken":
                    this.QuestionToken = childNode;
                    break;

                case "type":
                    this.Type = childNode;
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
