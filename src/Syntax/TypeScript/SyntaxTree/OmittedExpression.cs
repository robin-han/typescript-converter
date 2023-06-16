using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.OmittedExpression)]
    public class OmittedExpression : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.OmittedExpression; }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
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
    }
}
