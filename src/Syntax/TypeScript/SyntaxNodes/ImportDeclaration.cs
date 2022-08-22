using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
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
                string path = System.IO.Path.Combine(dir, this.ModuleSpecifier.Text);
                return System.IO.Path.GetFullPath(path);
            }
        }

        public Document FromDocument
        {
            get
            {
                return this.Project.GetDocumentByPath(this.ModulePath);
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.ImportClause = null;
            this.ModuleSpecifier = null;
        }

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

        public Node GetTypeDefinition(string typeName)
        {
            Project project = this.Project;
            Document fromDoc = project.GetDocumentByPath(this.ModulePath);
            if (fromDoc == null)
            {
                return null;
            }

            ImportClause importClause = this.ImportClause as ImportClause; // import name from './abc';
            if (importClause.Name != null && importClause.Name.Text == typeName)
            {
                return fromDoc.GetExportDefaultTypeDefinition();
            }

            ImportSpecifier specifier = this.GetImportSpecifier(typeName);
            if (specifier != null)
            {
                string definitionName = specifier.DefinitionName;
                if (definitionName == "default")
                {
                    return fromDoc.GetExportDefaultTypeDefinition();
                }
                else
                {
                    return fromDoc.GetExportTypeDefinition(definitionName);
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

