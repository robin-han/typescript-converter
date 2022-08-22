using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TupleType)]
    public class TupleType : Node
    {
        public TupleType()
        {
            this.ElementTypes = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TupleType; }
        }

        public List<Node> ElementTypes
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
                case "elementTypes":
                    this.ElementTypes.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
