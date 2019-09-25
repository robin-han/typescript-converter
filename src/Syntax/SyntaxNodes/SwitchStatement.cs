using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class SwitchStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SwitchStatement; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public CaseBlock CaseBlock
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
            this.CaseBlock = null;
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

                case "caseBlock":
                    this.CaseBlock = childNode as CaseBlock;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

