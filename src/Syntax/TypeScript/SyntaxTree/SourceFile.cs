using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SourceFile)]
    public class SourceFile : Node
    {
        private string text;

        public SourceFile()
        {
            this.Statements = new List<Node>();
            this.FileName = string.Empty;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SourceFile; }
        }

        public override string Text
        {
            get
            {
                return this.text;
            }
            internal set
            {
                this.text = value;
            }
        }

        public string FileName
        {
            get;
            private set;
        }

        public List<Node> Statements
        {
            get;
            private set;
        }

        public EndOfFileToken EndOfFileToken
        {
            get;
            private set;
        }

        public List<Node> TypeAliases
        {
            get
            {
                return this.Statements.FindAll(n => n.IsTypeAliasType());
            }
        }

        public List<Node> ImportDeclarations
        {
            get
            {
                return this.Statements.FindAll(n => n.Kind == NodeKind.ImportDeclaration);
            }
        }

        public List<Node> ExportDeclarations
        {
            get
            {
                return this.Statements.FindAll(n => n.Kind == NodeKind.ExportDeclaration);
            }
        }

        public Node ExportAssignment
        {
            get
            {
                return this.Statements.Find(n => n.Kind == NodeKind.ExportAssignment);
            }
        }

        public List<Node> ModuleDeclarations
        {
            get
            {
                return this.Statements.FindAll(n => n.Kind == NodeKind.ModuleDeclaration);
            }
        }

        public string Namespace
        {
            get
            {
                List<Node> moduleDeclarations = this.ModuleDeclarations;
                if (moduleDeclarations.Count > 0)
                {
                    List<string> parts = new List<string>();
                    ModuleDeclaration module = (ModuleDeclaration)moduleDeclarations[0];
                    parts.Add(module.Name.Text);
                    while (module.Body.Kind == NodeKind.ModuleDeclaration)
                    {
                        module = (ModuleDeclaration)module.Body;
                        parts.Add(module.Name.Text);
                    }
                    return string.Join('.', parts);
                }
                return null;
            }
        }

        #region Ignored Properties
        private List<object> BindDiagnostics { get; set; }
        private int LanguageVersion { get; set; }
        private int LanguageVariant { get; set; }
        private bool IsDeclarationFile { get; set; }
        private int ScriptKind { get; set; }
        private object Pragmas { get; set; }
        private List<string> ReferencedFiles { get; set; }
        private List<string> TypeReferenceDirectives { get; set; }
        private List<string> LibReferenceDirectives { get; set; }
        private List<string> AmdDependencies { get; set; }
        private bool HasNoDefaultLib { get; set; }
        private int NodeCount { get; set; }
        private int IdentifierCount { get; set; }
        private object Identifiers { get; set; }
        private List<string> ParseDiagnostics { get; set; }
        private Node ExternalModuleIndicator { get; set; }
        #endregion

        #endregion

        #region Methods
        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            JToken jsonFileName = jsonObj["fileName"];
            this.FileName = jsonFileName == null ? string.Empty : jsonFileName.ToObject<string>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "endOfFileToken":
                    this.EndOfFileToken = childNode as EndOfFileToken;
                    break;

                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        //public override bool IsValidChild(Node childNode)
        //{
        //    return childNode.NodeName != "externalModuleIndicator";
        //}

        /// <summary>
        /// Get type definitions in the file.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetTypeDeclarations(bool includeDescendant = false)
        {
            List<Node> types = this.Statements.FindAll(n => n.IsTypeDeclaration());

            if (includeDescendant)
            {
                foreach (ModuleDeclaration m in this.ModuleDeclarations)
                {
                    ModuleBlock mBlock = m.GetModuleBlock();
                    if (mBlock != null)
                    {
                        types.AddRange(mBlock.TypeDeclarations);
                    }
                }
            }

            return types;
        }

        /// <summary>
        /// Get function definitions in the file.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetFunctionDeclarations(bool includeDescendant = false)
        {
            List<Node> funcs = this.Statements.FindAll(n =>
            {
                return n.Kind == NodeKind.FunctionDeclaration;
            });


            if (includeDescendant)
            {
                foreach (ModuleDeclaration m in this.ModuleDeclarations)
                {
                    ModuleBlock mBlock = m.GetModuleBlock();
                    if (mBlock != null)
                    {
                        funcs.AddRange(mBlock.Functions);
                    }
                }
            }

            return funcs;
        }

        /// <summary>
        /// Gets the export default type definition.
        /// </summary>
        /// <returns></returns>
        public Node GetExportDefaultModuleTypeDeclaration()
        {
            foreach (Node type in this.GetTypeDeclarations())
            {
                if (type.HasModify(NodeKind.ExportKeyword) && type.HasModify(NodeKind.DefaultKeyword))
                {
                    return type;
                }
            }

            foreach (ExportDeclaration export in this.ExportDeclarations)
            {
                Node defaultDefnition = export.GetDefaultTypeDeclaration();
                if (defaultDefnition != null)
                {
                    return defaultDefnition;
                }
            }

            Node exportAssignment = this.ExportAssignment;
            if (exportAssignment != null)
            {
                return ((ExportAssignment)exportAssignment).GetTypeDeclaration();
            }

            return null;
        }

        /// <summary>
        /// Gets export type definition.
        /// </summary>
        /// <param name="typeName">The export type name.</param>
        /// <returns>The definition.</returns>
        public Node GetExportModuleTypeDeclaration(string typeName)
        {
            Node definition = this.GetOwnModuleTypeDeclaration(typeName);
            if (definition != null && definition.HasModify(NodeKind.ExportKeyword))
            {
                return definition;
            }

            foreach (ExportDeclaration export in this.ExportDeclarations)
            {
                definition = export.GetTypeDeclaration(typeName);
                if (definition != null)
                {
                    return definition;
                }
            }

            return null;
        }

        public Node GetOwnModuleTypeDeclaration(string typeName)
        {
            foreach (Node type in this.GetTypeDeclarations())
            {
                if (type.GetValue("NameText") as string == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public Node GetImportModuleTypeDeclaration(string typeName)
        {
            foreach (ImportDeclaration import in this.ImportDeclarations)
            {
                Node defination = import.GetTypeDeclaration(typeName);
                if (defination != null)
                {
                    return defination;
                }
            }
            return null;
        }
        #endregion
    }
}
