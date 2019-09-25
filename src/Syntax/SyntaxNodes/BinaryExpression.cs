using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class BinaryExpression : Expression
    {
        private Node _left;
        private Node _operatorToken;
        private Node _right;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.BinaryExpression; }
        }

        public Node Left
        {
            get
            {
                return this._left;
            }
            internal set
            {
                this._left = value;
                if (this._left != null)
                {
                    this._left.Parent = this;
                }
            }
        }

        public Node OperatorToken
        {
            get
            {
                return this._operatorToken;
            }
            internal set
            {
                this._operatorToken = value;
                if (this._operatorToken != null)
                {
                    this._operatorToken.Parent = this;
                }
            }
        }

        public Node Right
        {
            get
            {
                return this._right;
            }
            internal set
            {
                this._right = value;
                if (this._right != null)
                {
                    this._right.Parent = this;
                }
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Left = null;
            this.OperatorToken = null;
            this.Right = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

