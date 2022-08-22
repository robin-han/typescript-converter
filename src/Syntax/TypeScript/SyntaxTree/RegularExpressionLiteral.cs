namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.RegularExpressionLiteral)]
    public class RegularExpressionLiteral : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.RegularExpressionLiteral; }
        }

        public string Pattern
        {
            get
            {
                string text = this.Text;
                int index = text.LastIndexOf('/');
                return text.Substring(1, index - 1);
            }
        }

        public string SearchFlags
        {
            get
            {
                string text = this.Text;
                int index = text.LastIndexOf('/');
                return text.Substring(index + 1);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
