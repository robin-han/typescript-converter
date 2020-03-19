using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ModuleBlock : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ModuleBlock; }
        }

        public List<Node> Statements
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
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Statements = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void AddTypeAlias(Node node, bool changeParent = true)
        {
            if (changeParent)
            {
                node.Parent = this;
            }
            this.TypeAliases.Add(node);
        }

    }
}

