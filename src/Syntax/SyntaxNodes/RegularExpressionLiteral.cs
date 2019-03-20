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
            private set;
        }

        public bool IgnoreCase
        {
            get;
            private set;
        }

        public bool Multiline
        {
            get;
            private set;
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

        protected override void NormalizeImp()
        {
            int lastIndex = this.Text.LastIndexOf('/');
            this.Pattern = this.Text.Substring(1, lastIndex - 1);

            string regOption = this.Text.Substring(lastIndex);
            if (regOption.Contains('i'))
            {
                this.IgnoreCase = true;
            }
            if (regOption.Contains('m'))
            {
                this.Multiline = true;
            }
        }

    }
}

