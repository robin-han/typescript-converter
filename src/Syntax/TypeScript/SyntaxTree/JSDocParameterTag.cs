using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocParameterTag)]
    public class JSDocParameterTag : Node
    {
        public JSDocParameterTag()
        {
            this.IsNameFirst = false;
            this.IsBracketed = false;
            this.Comment = string.Empty;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocParameterTag; }
        }

        public Node AtToken
        {
            get;
            private set;
        }

        public Node TagName
        {
            get;
            private set;
        }

        public Node TypeExpression
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public string Comment
        {
            get;
            private set;
        }

        private bool IsNameFirst
        {
            get;
            set;
        }

        private bool IsBracketed
        {
            get;
            set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            JToken jsonIsNameFirst = jsonObj["isNameFirst"];
            this.IsNameFirst = jsonIsNameFirst == null ? false : jsonIsNameFirst.ToObject<bool>();

            JToken jsonIsBracketed = jsonObj["isBracketed"];
            this.IsBracketed = jsonIsBracketed == null ? false : jsonIsBracketed.ToObject<bool>();

            JToken jsonComment = jsonObj["comment"];
            this.Comment = jsonComment == null ? string.Empty : jsonComment.ToObject<string>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "atToken":
                    this.AtToken = childNode;
                    break;

                case "tagName":
                    this.TagName = childNode;
                    break;

                case "typeExpression":
                    this.TypeExpression = childNode;
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
