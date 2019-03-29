using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ObjectLiteralExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ObjectLiteralExpression; }
        }

        public bool MultiLine
        {
            get;
            private set;
        }

        public List<Node> Properties
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            internal set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Properties = new List<Node>();

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "properties":
                    this.Properties.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

