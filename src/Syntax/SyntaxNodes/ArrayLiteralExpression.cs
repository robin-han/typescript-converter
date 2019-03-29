using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ArrayLiteralExpression : Expression
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayLiteralExpression; }
        }

        public bool MultiLine
        {
            get;
            private set;
        }

        public List<Node> Elements
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

            this.Elements = new List<Node>();

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "elements":
                    this.Elements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
      
    }
}

