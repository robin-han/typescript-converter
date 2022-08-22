using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.EnumDeclaration)]
    public class EnumDeclaration : TypeDeclaration
    {
        public EnumDeclaration()
        {
            this.Modifiers = new List<Node>();
            this.Members = new List<Node>();
            this.JsDoc = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.EnumDeclaration; }
        }

        public virtual List<Node> Members
        {
            get;
            protected set;
        }

        public string NameText
        {
            get
            {
                return (this.Name != null ? this.Name.Text : string.Empty);
            }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> JsDoc
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

        public bool IsBitFieldSet
        {
            get
            {
                List<int> values = new List<int>();
                foreach (EnumMember member in this.Members)
                {
                    Node initValue = member.Initializer;
                    if (initValue == null || initValue.Kind != NodeKind.NumericLiteral)
                    {
                        return false;
                    }

                    int value = int.Parse(initValue.Text);
                    if (value != 0 && value != 1)
                    {
                        values.Add(value);
                    }
                }
                //2, 4
                if (values.Count >= 2)
                {
                    values.Sort();
                    for (int i = 1; i < values.Count; i++)
                    {
                        if (values[i] != values[i - 1] * 2)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "name":
                    this.Name = childNode;
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "members":
                    this.Members.Add(childNode);
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
