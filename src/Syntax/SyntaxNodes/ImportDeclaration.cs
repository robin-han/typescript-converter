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
                string dir = System.IO.Path.GetDirectoryName(this.Document.Path);
                string path = System.IO.Path.Combine(dir, this.ModuleSpecifier.Text);
                return System.IO.Path.GetFullPath(path);
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
    }
}

