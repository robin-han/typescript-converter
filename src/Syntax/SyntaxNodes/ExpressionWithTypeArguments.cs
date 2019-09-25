using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ExpressionWithTypeArguments : Node
    {
        #region Fields
        private Node _expression;
        private Node _type = null;
        #endregion

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExpressionWithTypeArguments; }
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

        public List<Node> TypeArguments
        {
            get;
            private set;
        }

        public Node Type
        {
            get
            {
                if (this._type == null)
                {
                    if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                    {
                        this._type = this.Expression.ToQualifiedName();
                    }
                    else
                    {
                        this._type = this.Expression;
                    }
                }
                return this._type;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.TypeArguments = new List<Node>();
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

                case "typeArguments":
                    this.TypeArguments.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

