namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ArrayType)]
    public class ArrayType : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayType; }
        }

        public Node ElementType
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
                case "elementType":
                    this.ElementType = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void SetElementType(Node type, bool changeParent = true)
        {
            this.ElementType = type;
            if (changeParent && this.ElementType != null)
            {
                this.ElementType.Parent = this;
            }
        }
    }
}
