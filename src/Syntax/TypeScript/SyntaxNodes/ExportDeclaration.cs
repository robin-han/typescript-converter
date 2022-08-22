using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ExportDeclaration : Declaration
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

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.ExportClause = null;
            this.ModuleSpecifier = null;
        }

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

        public Node GetDefaultTypeDefinition()
        {
            return this.GetTypeDefinition("default");
        }

        public Node GetTypeDefinition(string typeName)
        {
            if (this.ModuleSpecifier == null)
            {
                return this.GetOwnTypeDefinition(typeName);
            }
            else
            {
                return this.GetFromTypeDefinition(typeName);
            }
        }

        private Node GetOwnTypeDefinition(string typeName)
        {
            ExportSpecifier specifier = this.GetExportSpecifier(typeName);
            if (specifier == null)
            {
                return null;
            }

            SourceFile sourceFile = this.Ancestor(NodeKind.SourceFile) as SourceFile;
            Node definition = sourceFile.GetOwnModuleTypeDefinition(specifier.DefinitionName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDefinition(specifier.DefinitionName);
            if (definition != null)
            {
                return definition;
            }

            return null;
        }

        private Node GetFromTypeDefinition(string typeName)
        {
            Document fromDoc = this.Project.GetDocumentByPath(this.ModulePath);
            if (fromDoc == null)
            {
                return null;
            }

            if (this.ExportClause == null) // export * from './abc'
            {
                return fromDoc.GetExportTypeDefinition(typeName);
            }

            ExportSpecifier specifier = this.GetExportSpecifier(typeName);
            if (specifier != null)
            {
                return fromDoc.GetExportTypeDefinition(specifier.DefinitionName);
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

