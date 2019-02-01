using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ForStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Condition
        {
            get;
            private set;
        }

        public Node Incrementor
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }

        public List<Node> Initializers
        {
            get;
            private set;
        }

        public List<Node> Incrementors
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Initializer = null;
            this.Condition = null;
            this.Incrementor = null;
            this.Statement = null;

            this.Initializers = new List<Node>();
            this.Incrementors = new List<Node>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "condition":
                    this.Condition = childNode;
                    break;

                case "incrementor":
                    this.Incrementor = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            //separate by comma
            Node initializer = this.Initializer;
            if (initializer != null)
            {
                if (initializer.Kind == NodeKind.BinaryExpression)
                {
                    this.Initializers.AddRange(this.GetExpressions(initializer as BinaryExpression));
                }
                else
                {
                    this.Initializers.Add(initializer);
                }
            }

            Node incrementor = this.Incrementor;
            if (incrementor != null)
            {
                if (incrementor.Kind == NodeKind.BinaryExpression)
                {
                    this.Incrementors.AddRange(this.GetExpressions(incrementor as BinaryExpression));
                }
                else
                {
                    this.Incrementors.Add(incrementor);
                }
            }
        }

        private List<Node> GetExpressions(BinaryExpression exp)
        {
            List<Node> expressions = new List<Node>();

            Queue<BinaryExpression> queue = new Queue<BinaryExpression>();
            queue.Enqueue(exp);
            while (queue.Count > 0)
            {
                BinaryExpression binary = queue.Dequeue();
                if (binary.OperatorToken.Kind != NodeKind.CommaToken)
                {
                    expressions.Add(binary);
                    continue;
                }

                //
                Node left = binary.Left;
                Node right = binary.Right;

                if (left.Kind == NodeKind.BinaryExpression)
                {
                    queue.Enqueue(left as BinaryExpression);
                }
                else
                {
                    expressions.Add(left);
                }

                if (right.Kind == NodeKind.BinaryExpression)
                {
                    queue.Enqueue(right as BinaryExpression);
                }
                else
                {
                    expressions.Add(right);
                }
            }
            expressions.Reverse();

            return expressions;
        }

    }
}

