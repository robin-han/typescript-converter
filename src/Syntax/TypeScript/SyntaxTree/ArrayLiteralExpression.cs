using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ArrayLiteralExpression)]
    public class ArrayLiteralExpression : Node
    {
        public ArrayLiteralExpression()
        {
            this.Elements = new List<Node>();
            this.MultiLine = false;
        }

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
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            JToken jsonMultiLine = jsonObj["multiLine"];
            this.MultiLine = jsonMultiLine == null ? false : jsonMultiLine.ToObject<bool>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

        public void SetType(Node type, bool changeParent = true)
        {
            this.Type = type;
            if (changeParent && this.Type != null)
            {
                this.Type.Parent = this;
            }
        }
    }
}
