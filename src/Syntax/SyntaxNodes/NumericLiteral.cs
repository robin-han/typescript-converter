using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class NumericLiteral : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NumericLiteral; }
        }

        #region Ignored Properties
        private int NumericLiteralFlags { get; set; }
        #endregion
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

