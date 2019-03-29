using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class RegularExpressionLiteral : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.RegularExpressionLiteral; }
        }

        public string Pattern
        {
            get;
            internal set;
        }

        public bool IgnoreCase
        {
            get;
            internal set;
        }

        public bool Multiline
        {
            get;
            internal set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Pattern = "";
            this.IgnoreCase = false;
            this.Multiline = false;
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

