using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertyAccessExpression : Expression
    {
        private Node _expression;
        private Node _name;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyAccessExpression; }
        }

        public Node Expression
        {
            get
            {
                return this._expression;
            }
            internal set
            {
                this._expression = value;
                if (this._expression != null)
                {
                    this._expression.Parent = this;
                }
            }
        }

        public Node Name
        {
            get
            {
                return this._name;
            }
            internal set
            {
                this._name = value;
                if (this._name != null)
                {
                    this._name.Parent = this;
                }
            }
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

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

