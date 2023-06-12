using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ArrowFunction)]
    public class ArrowFunction : Node
    {
        public ArrowFunction()
        {
            this.Parameters = new List<Node>();
            this.Modifiers = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrowFunction; }
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            private set;
        }

        public Node EqualsGreaterThanToken
        {
            get;
            private set;
        }

        public Node Body
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Parameters = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Type = null;
            this.EqualsGreaterThanToken = null;
            this.Body = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "equalsGreaterThanToken":
                    this.EqualsGreaterThanToken = childNode;
                    break;

                case "body":
                    this.Body = childNode;
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
