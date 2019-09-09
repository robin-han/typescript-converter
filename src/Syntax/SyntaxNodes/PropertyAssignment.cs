using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertyAssignment : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyAssignment; }
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node Initializer
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.Initializer = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "name":
                    this.Name = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

