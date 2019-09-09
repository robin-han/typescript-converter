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
        #endregion

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Statements = new List<Node>();
            this.EndOfFileToken = null;

            JToken jsonFileName = jsonObj["fileName"];
            this.FileName = jsonFileName == null ? "" : jsonFileName.ToObject<string>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

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
    }
}

