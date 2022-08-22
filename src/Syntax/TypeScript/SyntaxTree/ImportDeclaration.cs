namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ImportDeclaration)]
    public class ImportDeclaration : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ImportDeclaration; }
        }

        public Node ImportClause
        {
            get;
            private set;
        }

        public Node ModuleSpecifier
        {
            get;
            private set;
        }

        public string ModulePath
        {
            get
            {
                if (this.ModuleSpecifier == null)
                {
                    return string.Empty;
                }

                string dir = System.IO.Path.GetDirectoryName(this.Document.Path);
                return System.IO.Path.Combine(dir, this.ModuleSpecifier.Text);
            }
        }

        public Document FromDocument
        {
            get
            {
                return this.Document.Project.GetDocument(this.ModulePath);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "importClause":
                    this.ImportClause = childNode;
                    break;

                case "moduleSpecifier":
                    this.ModuleSpecifier = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public Node GetTypeDeclaration(string typeName)
        {
            Project project = this.Document.Project;
            Document fromDoc = project.GetDocument(this.ModulePath);
            if (fromDoc == null)
            {
                return null;
            }

            ImportClause importClause = this.ImportClause as ImportClause; // import name from './abc';
            if (importClause.Name != null && importClause.Name.Text == typeName)
            {
                return fromDoc.GetExportDefaultTypeDeclaration();
            }

            ImportSpecifier specifier = this.GetImportSpecifier(typeName);
            if (specifier != null)
            {
                string definitionName = specifier.DefinitionName;
                if (definitionName == "default")
                {
                    return fromDoc.GetExportDefaultTypeDeclaration();
                }
                else
                {
                    return fromDoc.GetExportTypeDeclaration(definitionName);
                }
            }
            return null;
        }

        private ImportSpecifier GetImportSpecifier(string typeName)
        {
            ImportClause importClause = this.ImportClause as ImportClause;
            NamedImports namedImports = importClause.NamedBindings as NamedImports;
            if (namedImports != null)
            {
                foreach (ImportSpecifier specifier in namedImports.Elements)
                {
                    if (specifier.Name.Text == typeName)
                    {
                        return specifier;
                    }
                }
            }
            return null;
        }
    }
}
