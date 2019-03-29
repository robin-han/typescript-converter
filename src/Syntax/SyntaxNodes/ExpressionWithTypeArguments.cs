using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ExpressionWithTypeArguments : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExpressionWithTypeArguments; }
        }

        public Node Expression
        {
            get;
            internal set;
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
                if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                {
                    return this.ToQualifiedName(this.Expression as PropertyAccessExpression);
                }
                return this.Expression;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.TypeArguments = new List<Node>();
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

