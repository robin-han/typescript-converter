using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ExpressionStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExpressionStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            if (this.IsArrayClearStatement()) // change array.length = 0 to array.clear
            {
                BinaryExpression binaryExpr = this.Expression as BinaryExpression;
                CallExpression callExpr = this.CreateNode(NodeKind.CallExpression) as CallExpression;
                callExpr.Pos = binaryExpr.Pos;
                callExpr.End = binaryExpr.End;
                callExpr.Expression = binaryExpr.Left;
                (callExpr.Expression as PropertyAccessExpression).Name.Text = "clear";

                this.Expression = callExpr;
            }
        }

        private bool IsArrayClearStatement()
        {
            if (this.Expression.Kind != NodeKind.BinaryExpression)
            {
                return false;
            }

            BinaryExpression binaryExpr = this.Expression as BinaryExpression;
            if (binaryExpr.Left.Kind != NodeKind.PropertyAccessExpression)
            {
                return false;
            }

            PropertyAccessExpression left = binaryExpr.Left as PropertyAccessExpression;
            return (left.Name.Text == "length" && binaryExpr.OperatorToken.Kind == NodeKind.EqualsToken && binaryExpr.Right.Text == "0");
        }
    }
}

