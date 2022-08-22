using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.GetSetAccessor)]
    public class GetSetAccessor : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.GetSetAccessor; }
        }

        public GetAccessor GetAccessor
        {
            get;
            private set;
        }

        public SetAccessor SetAccessor
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get
            {
                return this.GetAccessor.Modifiers;
            }
        }

        public List<Node> JsDoc
        {
            get
            {
                return this.GetAccessor.JsDoc;
            }
        }

        public Node Name
        {
            get
            {
                return this.GetAccessor.Name;
            }
        }

        public Node Type
        {
            get;
            private set;
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            NodeKind kind = childNode.Kind;
            switch (kind)
            {
                case NodeKind.GetAccessor:
                    this.GetAccessor = childNode as GetAccessor;
                    break;

                case NodeKind.SetAccessor:
                    this.SetAccessor = childNode as SetAccessor;
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
