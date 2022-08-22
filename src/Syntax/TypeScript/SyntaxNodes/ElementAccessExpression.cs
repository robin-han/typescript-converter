using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ElementAccessExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ElementAccessExpression; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node ArgumentExpression
        {
            get;
            private set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.ArgumentExpression = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                case "argumentExpression":
                    this.ArgumentExpression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

