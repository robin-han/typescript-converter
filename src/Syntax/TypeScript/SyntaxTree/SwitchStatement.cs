namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SwitchStatement)]
    public class SwitchStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SwitchStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public CaseBlock CaseBlock
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

                case "caseBlock":
                    this.CaseBlock = childNode as CaseBlock;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
