using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CaseBlock)]
    public class CaseBlock : Node
    {
        public CaseBlock()
        {
            this.Clauses = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CaseBlock; }
        }

        public List<Node> Clauses
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
                case "clauses":
                    this.Clauses.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
