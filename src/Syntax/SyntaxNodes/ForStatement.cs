using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ForStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Condition
        {
            get;
            private set;
        }

        public Node Incrementor
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }

        public List<Node> Initializers
        {
            get;
            private set;
        }

        public List<Node> Incrementors
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Initializer = null;
            this.Condition = null;
            this.Incrementor = null;
            this.Statement = null;

            this.Initializers = new List<Node>();
            this.Incrementors = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "condition":
                    this.Condition = childNode;
                    break;

                case "incrementor":
                    this.Incrementor = childNode;
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

