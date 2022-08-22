namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.BindingElement)]
    public class BindingElement : Node
    {
        #region Proert
        public override NodeKind Kind
        {
            get { return NodeKind.BindingElement; }
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
