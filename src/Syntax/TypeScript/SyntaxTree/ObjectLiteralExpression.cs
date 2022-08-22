using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ObjectLiteralExpression)]
    public class ObjectLiteralExpression : Node
    {
        public ObjectLiteralExpression()
        {
            this.Properties = new List<Node>();
            this.MultiLine = false;
        }

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
                case "properties":
                    this.Properties.Add(childNode);
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
