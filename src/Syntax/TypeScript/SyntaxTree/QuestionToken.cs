namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.QuestionToken)]
    public class QuestionToken : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.QuestionToken; }
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
