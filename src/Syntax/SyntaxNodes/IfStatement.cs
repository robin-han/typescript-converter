using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class IfStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IfStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node ThenStatement
        {
            get;
            private set;
        }

        public Node ElseStatement
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.ThenStatement = null;
            this.ElseStatement = null;
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
                case "thenStatement":
                    this.ThenStatement = childNode;
                    break;
                case "elseStatement":
                    this.ElseStatement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

