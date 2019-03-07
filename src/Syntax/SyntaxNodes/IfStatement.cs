using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class IfStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IfStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node ThenStatement
        {
            get;
            private set;
        }

        public Node ElseStatement
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.ThenStatement = null;
            this.ElseStatement = null;
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
                case "thenStatement":
                    this.ThenStatement = childNode;
                    break;
                case "elseStatement":
                    this.ElseStatement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            this.NormalizeInstanceOf();
        }

        private void NormalizeInstanceOf()
        {
            List<Node> instanceofs = this.Expression.DescendantsAndSelf(n =>
                n.Kind == NodeKind.BinaryExpression &&
                (n as BinaryExpression).OperatorToken.Kind == NodeKind.InstanceOfKeyword);
            if (instanceofs.Count == 0)
            {
                return;
            }

            BinaryExpression instanceof = instanceofs[0] as BinaryExpression;
            string variableName = instanceof.Left.Text;
            string typeName = instanceof.Right.Text;
            List<Node> asNodes = this.ThenStatement.Descendants(n => n.Kind == NodeKind.Identifier && n.Text == variableName);
            foreach (Identifier asNode in asNodes)
            {
                if (asNode.Parent.Kind == NodeKind.PropertyAccessExpression)
                {
                    if (asNode == (asNode.Parent as PropertyAccessExpression).Expression)
                    {
                        asNode.As = typeName;
                    }
                }
                else
                {
                    asNode.As = typeName;
                }
            }
        }

    }
}

