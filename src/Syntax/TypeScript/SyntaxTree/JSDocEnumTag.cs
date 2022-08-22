namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocEnumTag)]
    public class JSDocEnumTag : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocEnumTag; }
        }

        public Node AtToken
        {
            get;
            private set;
        }

        public Node TagName
        {
            get;
            private set;
        }

        public Node TypeExpression
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
                case "atToken":
                    this.AtToken = childNode;
                    break;

                case "tagName":
                    this.TagName = childNode;
                    break;

                case "typeExpression":
                    this.TypeExpression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
