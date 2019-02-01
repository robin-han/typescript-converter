using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class NewExpression : Expression
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NewExpression; }
        }

        private Node Expression
        {
            get;
            set;
        }

        public List<Node> TypeArguments
        {
            get;
            private set;
        }

        public List<Node> Arguments
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
                    this._type = this.InferType();
                }
                return this._type;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.Arguments = new List<Node>();
            this.TypeArguments = new List<Node>();
            this._type = null;

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

                case "typeArguments":
                    this.TypeArguments.Add(childNode);
                    break;

                case "arguments":
                    this.Arguments.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                return this.PropertyAccessExpressionToQualifiedName(this.Expression);
            }
            return this.Expression;
        }


    }
}

