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
            get
            {
                string text = this.Text;
                int index = text.LastIndexOf('/');
                return text.Substring(1, index - 1);
            }
        }

        public string SearchFlags
        {
            get
            {
                string text = this.Text;
                int index = text.LastIndexOf('/');
                return text.Substring(index + 1);
            }
        }
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

