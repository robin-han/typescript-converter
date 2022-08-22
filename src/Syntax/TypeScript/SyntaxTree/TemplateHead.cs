namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TemplateHead)]
    public class TemplateHead : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TemplateHead; }
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
