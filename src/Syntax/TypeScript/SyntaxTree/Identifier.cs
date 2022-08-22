using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.Identifier)]
    public class Identifier : Node
    {
        public Identifier()
        {
            this.EscapedText = string.Empty;
            this.As = string.Empty;
        }

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
        public int JsDocDotPos
        {
            get;
            private set;
        }

        public string As
        {
            get;
            internal set;
        }

        public Node Type
        {
            // TODO:
            get;
            set;
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
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override string GetText()
        {
            return this.EscapedText;
        }
    }
}
