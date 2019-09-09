using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class Block : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.Block; }
        }

        public bool MultiLine
        {
            get;
            private set;
        }
        public List<Node> Statements
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
            this.Statements = new List<Node>();

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

