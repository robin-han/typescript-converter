using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ImportEqualsDeclaration)]
    public class ImportEqualsDeclaration : Node
    {
        public ImportEqualsDeclaration()
        {
            this.Modifiers = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ImportEqualsDeclaration; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public virtual Node Name
        {
            get;
            private set;
        }

        public Node ModuleReference
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
                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "moduleReference":
                    this.ModuleReference = childNode;
                    break;
                    
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}
