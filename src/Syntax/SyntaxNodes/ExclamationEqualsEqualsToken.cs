using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ExclamationEqualsEqualsToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.ExclamationEqualsEqualsToken; }
        }

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

