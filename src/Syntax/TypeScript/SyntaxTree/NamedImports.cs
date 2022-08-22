using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.NamedImports)]
    public class NamedImports : Node
    {
        public NamedImports()
        {
            this.Elements = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NamedImports; }
        }

        public List<Node> Elements
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
                case "elements":
                    this.Elements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
