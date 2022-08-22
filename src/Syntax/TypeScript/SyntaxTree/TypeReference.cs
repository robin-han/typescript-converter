using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeReference)]
    public class TypeReference : Node
    {
        public TypeReference()
        {
            this.TypeArguments = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeReference; }
        }

        public Node TypeName
        {
            get;
            private set;
        }

        public List<Node> TypeArguments
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
                case "typeName":
                    this.TypeName = childNode;
                    break;

                case "typeArguments":
                    this.TypeArguments.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override string GetText()
        {
            return this.TypeName.Text;
        }

        public void SetTypeName(Node typeName, bool changeParent = true)
        {
            this.TypeName = typeName;
            if (changeParent && this.TypeName != null)
            {
                this.TypeName.Parent = this;
            }
        }
    }
}
