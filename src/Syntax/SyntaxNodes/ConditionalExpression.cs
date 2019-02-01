using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ConditionalExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ConditionalExpression; }
        }

        public Node Condition
        {
            get;
            private set;
        }

        public Node QuestionToken
        {
            get;
            private set;
        }

        public Node WhenTrue
        {
            get;
            private set;
        }

        public Node ColonToken
        {
            get;
            private set;
        }

        public Node WhenFalse
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Condition = null;
            this.QuestionToken = null;
            this.WhenTrue = null;
            this.ColonToken = null;
            this.WhenFalse = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "condition":
                    this.Condition = childNode;
                    break;
                case "questionToken":
                    this.QuestionToken = childNode;
                    break;
                case "whenTrue":
                    this.WhenTrue = childNode;
                    break;
                case "colonToken":
                    this.ColonToken = childNode;
                    break;
                case "whenFalse":
                    this.WhenFalse = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

