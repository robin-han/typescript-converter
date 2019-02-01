using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class DoStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.DoStatement; }
        }

        public Node Statement
        {
            get;
            private set;
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

            this.Statement = null;
            this.Expression = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statement":
                    this.Statement = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

