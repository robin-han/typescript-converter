using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class CallExpression : Expression
    {
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
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.TypeArguments = new List<Node>();
            this.Arguments = new List<Node>();
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

