using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class PostfixUnaryExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.PostfixUnaryExpression; }
        }

        public Node Operand
        {
            get;
            private set;
        }

        public NodeKind Operator
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Operand = null;

            JToken jsonOperator = jsonObj["operator"];
            this.Operator = jsonOperator == null ? NodeKind.Unknown : (NodeKind)(jsonOperator.ToObject<int>() + 1); //typescropt does not give the correctly value  
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "operand":
                    this.Operand = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

