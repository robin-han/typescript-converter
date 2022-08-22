namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TemplateSpan)]
    public class TemplateSpan : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TemplateSpan; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Literal
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

                case "literal":
                    this.Literal = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
