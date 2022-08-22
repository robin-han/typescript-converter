using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.Constructor)]
    public class Constructor : Node
    {
        public Constructor()
        {
            this.Modifiers = new List<Node>();
            this.Parameters = new List<Node>();
            this.JsDoc = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.Constructor; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }

        public Node Base
        {
            get
            {
                if (this.Body.Statements.Count > 0 && this.IsBaseStatement(this.Body.Statements[0]))
                {
                    return this.Body.Statements[0];
                }
                return null;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "body":
                    this.Body = (Block)childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        private bool IsBaseStatement(Node statement)
        {
            if (statement.Kind != NodeKind.ExpressionStatement)
            {
                return false;
            }

            ExpressionStatement expStatement = statement as ExpressionStatement;
            if (expStatement.Expression.Kind != NodeKind.CallExpression)
            {
                return false;
            }

            CallExpression callExp = expStatement.Expression as CallExpression;
            if (callExp.Expression.Kind != NodeKind.SuperKeyword)
            {
                return false;
            }

            return true;
        }
    }
}
