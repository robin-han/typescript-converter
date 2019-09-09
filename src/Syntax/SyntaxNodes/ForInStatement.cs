using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ForInStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForInStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Initializer = null;
            this.Expression = null;
            this.Statement = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

