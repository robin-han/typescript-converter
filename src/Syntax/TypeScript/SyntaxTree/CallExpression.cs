using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.CallExpression)]
    public class CallExpression : Node
    {
        public CallExpression()
        {
            this.TypeArguments = new List<Node>();
            this.Arguments = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CallExpression; }
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

        public List<Node> Arguments
        {
            get;
            private set;
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
                if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                {
                    return ((PropertyAccessExpression)this.Expression).Parts;
                }
                else
                {
                    return new List<string>() { this.Expression.Text };
                }
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

                case "arguments":
                    this.Arguments.Add(childNode);
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

        public bool IsTypePredicate(out FunctionDeclaration typePredicateFunc)
        {
            typePredicateFunc = null;
            string text = this.Expression.Text;
            int index = text.LastIndexOf('.');
            if (index >= 0)
            {
                text = text.Substring(index + 1);
            }

            if ((text.StartsWith("_is") || text.StartsWith("is")) && this.Arguments.Count == 1)
            {
                typePredicateFunc = (FunctionDeclaration)this.Document.Project.Functions.Find(n =>
                {
                    FunctionDeclaration func = (FunctionDeclaration)n;
                    return (func.Name.Text == text && func.Type != null && func.Type.Kind == NodeKind.TypePredicate);
                });
            }
            return typePredicateFunc != null;
        }

        #region TypeArguments
        public void AddTypeArgument(Node typeArgument, bool changeParent = true)
        {
            if (changeParent)
            {
                typeArgument.Parent = this;
            }
            this.TypeArguments.Add(typeArgument);
        }
        #endregion

        #region Arguments
        public void AddArgument(Node argument, bool changeParent = true)
        {
            if (changeParent)
            {
                argument.Parent = this;
            }
            this.Arguments.Add(argument);
        }

        public void AddArguments(IEnumerable<Node> arguments, bool changeParent = true)
        {
            if (changeParent)
            {
                foreach (var argument in arguments)
                {
                    argument.Parent = this;
                }
            }
            this.Arguments.AddRange(arguments);
        }

        public void RemoveArgumentAt(int index)
        {
            this.Arguments.RemoveAt(index);
        }

        public void ClearArguments()
        {
            this.Arguments.Clear();
        }
        #endregion

    }
}
