using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class JSDocTypeExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocTypeExpression; }
        }

        public Node Type
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "type":
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

