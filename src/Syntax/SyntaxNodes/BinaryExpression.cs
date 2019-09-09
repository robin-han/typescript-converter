using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class BinaryExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.BinaryExpression; }
        }

        public Node Left
        {
            get;
            internal set;
        }

        public Node OperatorToken
        {
            get;
            internal set;
        }

        public Node Right
        {
            get;
            internal set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Left = null;
            this.OperatorToken = null;
            this.Right = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "left":
                    this.Left = childNode;
                    break;

                case "operatorToken":
                    this.OperatorToken = childNode;
                    break;

                case "right":
                    this.Right = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

