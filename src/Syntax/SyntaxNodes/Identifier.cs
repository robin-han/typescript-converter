using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class Identifier : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.Identifier; }
        }

        public string EscapedText
        {
            get;
            private set;
        }

        public string As
        {
            get;
            internal set;
        }

        #region Ignored Properties
        private int OriginalKeywordKind { get; set; }
        #endregion

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            JToken jsonEscapedText = jsonObj["escapedText"];
            this.EscapedText = jsonEscapedText == null ? string.Empty : jsonEscapedText.ToObject<string>();
            this.As = string.Empty;
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

