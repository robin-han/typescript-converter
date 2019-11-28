using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class FunctionExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.FunctionExpression; }
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }

        private int ModifierFlagsCache { get; set; }
        private Node Type { get; set; }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Parameters = new List<Node>();
            this.Body = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "body":
                    this.Body = (Block)childNode;
                    break;

                default:
                    // this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

