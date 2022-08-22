using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class PropertyAccessExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PropertyAccessExpression; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
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

        public void SetExpression(Node expression, bool changeParent = true)
        {
            this.Expression = expression;
            if (changeParent && this.Expression != null)
            {
                this.Expression.Parent = this;
            }
        }

        public void SetName(Node name, bool changeParent = true)
        {
            this.Name = name;
            if (changeParent && this.Name != null)
            {
                this.Name.Parent = this;
            }
        }
    }
}

