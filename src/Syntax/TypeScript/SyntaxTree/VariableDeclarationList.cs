using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.VariableDeclarationList)]
    public class VariableDeclarationList : Node
    {
        public VariableDeclarationList()
        {
            this.Declarations = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VariableDeclarationList; }
        }

        public List<Node> Declarations
        {
            get;
            private set;
        }

        public Node Type
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
                case "declarations":
                    this.Declarations.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void SetType(Node type, bool changeParent = true)
        {
            this.Type = type;
            if (changeParent && this.Type != null)
            {
                this.Type.Parent = this;
            }
        }
    }
}
