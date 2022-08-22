using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ConstructorType)]
    public class ConstructorType : Node
    {
        public ConstructorType()
        {
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
            this.Parameters = new List<Node>();

        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ConstructorType; }
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }


        public List<Node> Parameters
        {
            get;
            private set;
        }

        public Node Name
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
                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "parameters":
                    this.Parameters.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
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
