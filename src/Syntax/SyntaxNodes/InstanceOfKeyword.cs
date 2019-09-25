using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class InstanceOfKeyword: Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.InstanceOfKeyword; }
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

