using System.Collections.Generic;
using System.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.UnionType)]
    public class UnionType : Node
    {
        public UnionType()
        {
            this.Types = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.UnionType; }
        }

        public List<Node> Types
        {
            get;
            private set;
        }

        public bool HasNullType
        {
            get
            {
                return this.Types.Any(type => type.Kind == NodeKind.NullKeyword);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "types":
                    this.Types.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
