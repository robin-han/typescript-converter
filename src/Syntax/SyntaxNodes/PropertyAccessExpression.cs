using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertyAccessExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyAccessExpression; }
        }

        public Node Expression
        {
            get;
            internal set;
        }

        public Node Name
        {
            get;
            internal set;
        }

        public string As
        {
            get;
            internal set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.Name = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

