using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ExpressionWithTypeArguments)]
    public class ExpressionWithTypeArguments : Node
    {
        public ExpressionWithTypeArguments()
        {
            this.TypeArguments = new List<Node>();
        }

        #region Fields
        private Node _type = null;
        #endregion

        #region Propertie
        public override NodeKind Kind
        {
            get { return NodeKind.ExpressionWithTypeArguments; }
        }

        public Node Expression
        {
            get;
            private set;
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
                        Node type = this.Expression.ToQualifiedName();
                        type.Parent = this;
                        this._type = type;
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

        public void SetBase(Node baseNode, bool changeParent = true)
        {
            this.Expression = baseNode;
            if (changeParent && this.Expression != null)
            {
                this.Expression.Parent = this;
            }
        }
    }
}
