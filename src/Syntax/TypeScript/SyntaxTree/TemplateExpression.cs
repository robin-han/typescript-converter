using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TemplateExpression)]
    public class TemplateExpression : Node
    {
        public TemplateExpression()
        {
            this.TemplateSpans = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TemplateExpression; }
        }

        public Node Head
        {
            get;
            private set;
        }

        public List<Node> TemplateSpans
        {
            get;
            private set;
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "head":
                    this.Head = childNode;
                    break;

                case "templateSpans":
                    this.TemplateSpans.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
