using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.SetAccessor)]
    public class SetAccessor : Node
    {
        public SetAccessor()
        {
            this.Modifiers = new List<Node>();
            this.Parameters = new List<Node>();
            this.JsDoc = new List<Node>();
            this.Decorators = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SetAccessor; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Decorators
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public Block Body
        {
            get;
            private set;
        }

        public Node Type
        {
            get
            {
                return ((Parameter)this.Parameters[0]).Type;
            }
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

                case "parameters":
                    this.Parameters.Add(childNode as Parameter);
                    break;

                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "decorators":
                    this.Decorators.Add(childNode);
                    break;

                case "body":
                    this.Body = (Block)childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
