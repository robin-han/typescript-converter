using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class SourceFile : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SourceFile; }
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
                return this.Statements.FindAll(n => this.IsTypeAliasType(n));
            }
        }

        public List<Node> TypeDefinitions
        {
            get
            {
                return this.Statements.FindAll(n => this.IsTypeNode(n));
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

        public List<Node> MouduleDeclarations
        {
            get
            {
                return this.Statements.FindAll(n => n.Kind == NodeKind.ModuleDeclaration);
            }
        }

        public List<Node> TypeNodes
        {
            get
            {
                List<Node> typeNodes = new List<Node>();
                typeNodes.AddRange(this.Statements.FindAll(n =>
                {
                    return this.IsTypeNode(n);
                }));
                this.MouduleDeclarations.ForEach(m =>
                {
                    ModuleBlock mBlock = ((ModuleDeclaration)m).GetModuleBlock();
                    if (mBlock != null)
                    {
                        typeNodes.AddRange(mBlock.Statements.FindAll(n =>
                        {
                            return this.IsTypeNode(n);
                        }));
                    }
                });
                return typeNodes;
            }
        }

        public List<Node> GlobalFunctions
        {
            get
            {
                List<Node> funcs = new List<Node>();
                funcs.AddRange(this.Statements.FindAll(n =>
                {
                    return n.Kind == NodeKind.FunctionDeclaration;
                }));
                this.MouduleDeclarations.ForEach(m =>
                {
                    ModuleBlock mBlock = ((ModuleDeclaration)m).GetModuleBlock();
                    if (mBlock != null)
                    {
                        funcs.AddRange(mBlock.Statements.FindAll(n => n.Kind == NodeKind.FunctionDeclaration));
                    }
                });
                return funcs;
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

            this.Statements = new List<Node>();
            this.EndOfFileToken = null;

            JToken jsonFileName = jsonObj["fileName"];
            this.FileName = jsonFileName == null ? "" : jsonFileName.ToObject<string>();
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

        public override bool IsValidChild(Node childNode)
        {
            return childNode.NodeName != "externalModuleIndicator";
        }

        /// <summary>
        /// Gets the export default type definition.
        /// </summary>
        /// <returns></returns>
        public Node GetExportDefaultModuleTypeDefinition()
        {
            foreach (Node type in this.TypeDefinitions)
            {
                if (type.HasModify(NodeKind.ExportKeyword) && type.HasModify(NodeKind.DefaultKeyword))
                {
                    return type;
                }
            }

            foreach (ExportDeclaration export in this.ExportDeclarations)
            {
                Node defaultDefnition = export.GetDefaultTypeDefinition();
                if (defaultDefnition != null)
                {
                    return defaultDefnition;
                }
            }

            Node exportAssignment = this.ExportAssignment;
            if (exportAssignment != null)
            {
                return ((ExportAssignment)exportAssignment).GetTypeDefinition();
            }

            return null;
        }

        /// <summary>
        /// Gets export type definition.
        /// </summary>
        /// <param name="typeName">The export type name.</param>
        /// <returns>The definition.</returns>
        public Node GetExportModuleTypeDefinition(string typeName)
        {
            Node definition = this.GetOwnModuleTypeDefinition(typeName);
            if (definition != null && definition.HasModify(NodeKind.ExportKeyword))
            {
                return definition;
            }

            foreach (ExportDeclaration export in this.ExportDeclarations)
            {
                definition = export.GetTypeDefinition(typeName);
                if (definition != null)
                {
                    return definition;
                }
            }

            return null;
        }

        public Node GetOwnModuleTypeDefinition(string typeName)
        {
            foreach (Node type in this.TypeDefinitions)
            {
                if (type.GetValue("NameText") as string == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public Node GetImportModuleTypeDefinition(string typeName)
        {
            foreach (ImportDeclaration import in this.ImportDeclarations)
            {
                Node defination = import.GetTypeDefinition(typeName);
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

