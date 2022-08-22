namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.QualifiedName)]
    public class QualifiedName : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.QualifiedName; }
        }

        public Node Left
        {
            get;
            private set;
        }

        public Node Right
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
                case "left":
                    this.Left = childNode;
                    break;

                case "right":
                    this.Right = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override string GetText()
        {
            return string.Join('.', this.Left.Text, this.Right.Text);
        }
    }
}
