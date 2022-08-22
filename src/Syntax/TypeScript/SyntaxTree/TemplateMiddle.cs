namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TemplateMiddle)]
    public class TemplateMiddle : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TemplateMiddle; }
        }
        #endregion

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
    }
}
