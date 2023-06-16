﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.IntersectionType)]
    public class IntersectionType : Node
    {
        public IntersectionType()
        {
            this.Types = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IntersectionType; }
        }

        public List<Node> Types
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Types = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "types":
                    this.Types.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
