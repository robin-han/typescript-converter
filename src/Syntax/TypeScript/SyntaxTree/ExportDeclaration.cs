namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExportDeclaration)]
    public class ExportDeclaration : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExportDeclaration; }
        }

        public Node ExportClause
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
                string path = System.IO.Path.Combine(dir, this.ModuleSpecifier.Text);
                return System.IO.Path.Combine(path);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "exportClause":
                    this.ExportClause = childNode;
                    break;

                case "moduleSpecifier":
                    this.ModuleSpecifier = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public Node GetDefaultTypeDeclaration()
        {
            return this.GetTypeDeclaration("default");
        }

        public Node GetTypeDeclaration(string typeName)
        {
            if (this.ModuleSpecifier == null)
            {
                return this.GetOwnTypeDeclaration(typeName);
            }
            else
            {
                return this.GetFromTypeDeclaration(typeName);
            }
        }

        private Node GetOwnTypeDeclaration(string typeName)
        {
            ExportSpecifier specifier = this.GetExportSpecifier(typeName);
            if (specifier == null)
            {
                return null;
            }

            SourceFile sourceFile = this.Ancestor<SourceFile>();
            Node definition = sourceFile.GetOwnModuleTypeDeclaration(specifier.DefinitionName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDeclaration(specifier.DefinitionName);
            if (definition != null)
            {
                return definition;
            }

            return null;
        }

        private Node GetFromTypeDeclaration(string typeName)
        {
            Document fromDoc = this.Document.Project.GetDocument(this.ModulePath);
            if (fromDoc == null)
            {
                return null;
            }

            if (this.ExportClause == null) // export * from './abc'
            {
                return fromDoc.GetExportTypeDeclaration(typeName);
            }

            ExportSpecifier specifier = this.GetExportSpecifier(typeName);
            if (specifier != null)
            {
                return fromDoc.GetExportTypeDeclaration(specifier.DefinitionName);
            }
            return null;
        }

        private ExportSpecifier GetExportSpecifier(string typeName)
        {
            if (this.ExportClause == null)
            {
                return null;
            }

            NamedExports namedExports = (NamedExports)this.ExportClause;
            foreach (ExportSpecifier specifier in namedExports.Elements)
            {
                if (specifier.Name.Text == typeName)
                {
                    return specifier;
                }
            }

            return null;
        }
    }
}
