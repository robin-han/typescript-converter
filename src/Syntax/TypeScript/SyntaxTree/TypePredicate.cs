namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypePredicate)]
    public class TypePredicate : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypePredicate; }
        }

        public Node ParameterName
        {
            get;
            private set;
        }

        public Node Type
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
                case "parameterName":
                    this.ParameterName = childNode;
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
