using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax
{
    public class NoSubstitutionTemplateLiteral : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NoSubstitutionTemplateLiteral; }
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
