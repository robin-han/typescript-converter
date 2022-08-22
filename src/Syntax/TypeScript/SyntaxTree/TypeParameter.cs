namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeParameter)]
    public class TypeParameter : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeParameter; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node Constraint
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

                case "constraint":
                    this.Constraint = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
