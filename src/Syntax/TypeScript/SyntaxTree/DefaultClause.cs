using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.DefaultClause)]
    public class DefaultClause : Node
    {
        public DefaultClause()
        {
            this.Statements = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.DefaultClause; }
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
