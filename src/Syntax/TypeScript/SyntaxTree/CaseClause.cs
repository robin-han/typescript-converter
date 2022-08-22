using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CaseClause)]
    public class CaseClause : Node
    {
        public CaseClause()
        {
            this.Statements = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CaseClause; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public List<Node> Statements
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
                case "expression":
                    this.Expression = childNode;
                    break;

                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
