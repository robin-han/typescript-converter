namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.NamespaceImport)]
    public class NamespaceImport : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NamespaceImport; }
        }

        public Node Name
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
                case "name":
                    this.Name = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
