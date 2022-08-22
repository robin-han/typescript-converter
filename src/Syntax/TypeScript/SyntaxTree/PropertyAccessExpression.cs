using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.PropertyAccessExpression)]
    public class PropertyAccessExpression : Node
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

        public Node Type
        {
            // TODO:
            get;
            set;
        }

        /// <summary>
        /// Gets call expression's parts. such as: a.b() -> [a,b]
        /// </summary>
        public List<string> Parts
        {
            get
            {
                List<string> parts = new List<string>() { this.Name.Text };

                if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                {
                    parts.InsertRange(0, ((PropertyAccessExpression)this.Expression).Parts);
                }
                else if (this.Expression.Kind == NodeKind.CallExpression)
                {
                    parts.InsertRange(0, ((CallExpression)this.Expression).Parts);
                }
                else if (this.Expression.Kind == NodeKind.ElementAccessExpression)
                {
                    parts.InsertRange(0, ((ElementAccessExpression)this.Expression).Parts);
                }
                else
                {
                    parts.Insert(0, this.Expression.Text);
                }

                return parts;
            }
        }

        private List<string> GetParts(PropertyAccessExpression propAccess)
        {
            List<string> parts = new List<string>();
            if (propAccess.Expression.Kind == NodeKind.PropertyAccessExpression)
            {
                parts.Insert(0, propAccess.Name.Text);
                parts.InsertRange(0, GetParts((PropertyAccessExpression)propAccess.Expression));
            }
            else if (propAccess.Expression.Kind == NodeKind.CallExpression)
            {
                parts.Insert(0, propAccess.Name.Text);
                parts.InsertRange(0, ((CallExpression)propAccess.Expression).Parts);
            }
            else
            {
                parts.Insert(0, propAccess.Name.Text);
                parts.Insert(0, propAccess.Expression.Text);
            }
            return parts;
        }
        #endregion

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
