using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeLiteral)]
    public class TypeLiteral : Node
    {
        public TypeLiteral()
        {
            this.Members = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeLiteral; }
        }

        public List<Node> Members
        {
            get;
            private set;
        }

        public bool IsIndexSignature
        {
            get
            {
                return this.Members[0].Kind == NodeKind.IndexSignature;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "members":
                    this.Members.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
