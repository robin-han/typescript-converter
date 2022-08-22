namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocAugmentsTag)]
    public class JSDocAugmentsTag : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocAugmentsTag; }
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

        public Node Class
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

                case "class":
                    this.Class = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
