using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ImportSpecifier : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ImportSpecifier; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node PropertyName
        {
            get;
            private set;
        }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.PropertyName = null;
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

                case "propertyName":
                    this.PropertyName = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

