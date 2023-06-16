using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ModuleDeclaration)]
    public class ModuleDeclaration : Node
    {
        public ModuleDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ModuleDeclaration; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node Body
        {
            get;
            private set;
        }

        public bool IsExport
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ExportKeyword);
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.DefaultKeyword);
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "body":
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }


        public ModuleBlock GetModuleBlock()
        {
            ModuleDeclaration md = this;
            while (md != null)
            {
                if (md.Body.Kind == NodeKind.ModuleBlock)
                {
                    return md.Body as ModuleBlock;
                }
                md = md.Body as ModuleDeclaration;
            }
            return null;
        }
    }
}
