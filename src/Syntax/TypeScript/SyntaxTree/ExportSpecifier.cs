namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExportSpecifier)]
    public class ExportSpecifier : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExportSpecifier; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node PropertyName
        {
            get;
            private set;
        }

        public string DefinitionName
        {
            get
            {
                return (this.PropertyName != null ? this.PropertyName.Text : this.Name.Text);
            }
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

                case "propertyName":
                    this.PropertyName = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
