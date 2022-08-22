using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.JSDocComment)]
    public class JSDocComment : Node
    {
        public JSDocComment()
        {
            this.Tags = new List<Node>();
            this.Comment = string.Empty;
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocComment; }
        }

        public List<Node> Tags
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
                case "tags":
                    this.Tags.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
