namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ForOfStatement)]
    public class ForOfStatement : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForOfStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }

        public Node Identifier
        {
            get
            {
                VariableDeclaration variableDeclaration = this.GetVariableDeclaration();
                if (variableDeclaration != null)
                {
                    return variableDeclaration.Name;
                }
                return null;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        private VariableDeclaration GetVariableDeclaration()
        {
            VariableDeclarationList initializer = this.Initializer as VariableDeclarationList;
            if (initializer != null && initializer.Declarations.Count > 0)
            {
                return initializer.Declarations[0] as VariableDeclaration;
            }
            return null;
        }
    }
}
