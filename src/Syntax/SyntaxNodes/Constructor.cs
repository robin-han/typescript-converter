using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Constructor : Node
    {
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

        public Node Body
        {
            get;
            private set;
        }

        public Node BaseConstructor
        {
            get;
            private set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Modifiers = new List<Node>();
            this.Parameters = new List<Node>();
            this.JsDoc = new List<Node>();
            this.Body = null;

            this.BaseConstructor = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

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
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            if (!this.Modifiers.Exists(n => n.Kind == NodeKind.PublicKeyword || n.Kind == NodeKind.PrivateKeyword || n.Kind == NodeKind.ProtectedKeyword))
            {
                this.Modifiers.Add(this.CreateNode(NodeKind.PublicKeyword));
            }

            List<Node> statements = (this.Body as Block).Statements;
            for (int i = 0; i < statements.Count; i++)
            {
                Node statement = statements[i];
                if (this.IsBaseConstructor(statement))
                {
                    this.Body.Remove(statement);
                    this.BaseConstructor = this.CreateNode(statement.TsNode);
                    break;
                }
            }
        }

        private bool IsBaseConstructor(Node node)
        {
            if (node.Kind != NodeKind.ExpressionStatement)
            {
                return false;
            }

            ExpressionStatement expStatement = node as ExpressionStatement;
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

