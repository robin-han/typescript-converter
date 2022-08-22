using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.IndexSignature)]
    public class IndexSignature : Node
    {
        public IndexSignature()
        {
            this.Parameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IndexSignature; }
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

        public Node KeyType
        {
            get
            {
                if (Parameters.Count == 0)
                {
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);
                }
                return (this.Parameters[0] as Parameter).Type;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "parameters":
                    this.Parameters.Add(childNode);
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
