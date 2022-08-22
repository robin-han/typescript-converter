using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.TypeAliasDeclaration)]
    public class TypeAliasDeclaration : TypeDeclaration
    {
        public TypeAliasDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeAliasDeclaration; }
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

        public string NameText
        {
            get
            {
                return (this.Name != null ? this.Name.Text : string.Empty);
            }
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            private set;
        }

        public bool IsDelegate
        {
            get
            {
                return this.Type.Kind == NodeKind.FunctionType;
            }
        }

        public bool IsExport
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ExportKeyword);
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

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
