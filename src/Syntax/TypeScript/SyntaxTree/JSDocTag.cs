using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocTag)]
    public class JSDocTag : Node
    {
        public JSDocTag()
        {
            this.Comment = string.Empty;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocTag; }
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

        public string Comment
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

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

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override string GetText()
        {
            if (string.IsNullOrEmpty(this.Comment))
            {
                return base.GetText();
            }
            return "@" + this.TagName.Text + this.Comment;
        }
    }
}
