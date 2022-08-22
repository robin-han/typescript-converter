namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExportAssignment)]
    public class ExportAssignment : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExportAssignment; }
        }

        public Node Expression
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
                case "expression":
                    this.Expression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public Node GetTypeDeclaration() // export default AA;
        {
            string typeName = this.Expression.Text;

            SourceFile sourceFile = this.Ancestor<SourceFile>();
            Node definition = sourceFile.GetOwnModuleTypeDeclaration(typeName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDeclaration(typeName);
            if (definition != null)
            {
                return definition;
            }

            return null;
        }
    }
}
