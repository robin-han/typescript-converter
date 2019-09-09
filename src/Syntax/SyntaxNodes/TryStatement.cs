using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class TryStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TryStatement; }
        }

        public Node TryBlock
        {
            get;
            private set;
        }

        public Node CatchClause
        {
            get;
            private set;
        }

        public Node FinallyBlock
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.TryBlock = null;
            this.CatchClause = null;
            this.FinallyBlock = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "tryBlock":
                    this.TryBlock = childNode;
                    break;

                case "catchClause":
                    this.CatchClause = childNode;
                    break;

                case "finallyBlock":
                    this.FinallyBlock = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

