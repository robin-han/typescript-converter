using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.VariableStatement)]
    public class VariableStatement : Node
    {
        public VariableStatement()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VariableStatement; }
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

        public Node DeclarationList
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
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "declarationList":
                    this.DeclarationList = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
