using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ImportClause : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ImportClause; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node NamedBindings
        {
            get;
            set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.NamedBindings = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "name":
                    this.Name = childNode;
                    break;

                case "namedBindings":
                    this.NamedBindings = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

