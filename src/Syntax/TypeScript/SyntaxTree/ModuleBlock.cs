using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ModuleBlock)]
    public class ModuleBlock : Node
    {
        public ModuleBlock()
        {
            this.Statements = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ModuleBlock; }
        }

        public List<Node> Statements
        {
            get;
            private set;
        }

        public List<Node> TypeAliases
        {
            get
            {
                return this.Statements.FindAll(n => n.IsTypeAliasType());
            }
        }

        public List<Node> TypeDeclarations
        {
            get
            {
                return this.Statements.FindAll(n => n.IsTypeDeclaration());
            }
        }

        public List<Node> Functions
        {
            get
            {
                return this.Statements.FindAll(n => n.Kind == NodeKind.FunctionDeclaration);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public void AddTypeAlias(Node node, bool changeParent = true)
        {
            if (changeParent)
            {
                node.Parent = this;
            }
            this.TypeAliases.Add(node);
        }
    }
}
