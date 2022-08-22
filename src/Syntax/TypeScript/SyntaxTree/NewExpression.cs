using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.NewExpression)]
    public class NewExpression : Node
    {
        public NewExpression()
        {
            this.Arguments = new List<Node>();
            this.TypeArguments = new List<Node>();
        }

        #region Fields
        private Node _type = null;
        #endregion

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NewExpression; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        public List<Node> TypeArguments
        {
            get;
            private set;
        }

        public List<Node> Arguments
        {
            get;
            private set;
        }

        public Node Type
        {
            get
            {
                if (this._type == null)
                {
                    TypeReference type = (TypeReference)NodeHelper.CreateNode(NodeKind.TypeReference);

                    // TypeName
                    Node typeName;
                    if (this.Expression.Kind == NodeKind.PropertyAccessExpression)
                    {
                        typeName = this.Expression.ToQualifiedName();
                    }
                    else
                    {
                        typeName = NodeHelper.CreateNode(this.Expression.TsNode);
                    }
                    type.SetTypeName(typeName);

                    //TypeArguments
                    foreach (var typeArg in this.TypeArguments)
                    {
                        Node argNode = NodeHelper.CreateNode(typeArg.TsNode);
                        argNode.Parent = type;
                        type.TypeArguments.Add(argNode);
                    }

                    type.Parent = this;
                    this._type = type;
                }
                return this._type;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                case "typeArguments":
                    this.TypeArguments.Add(childNode);
                    break;

                case "arguments":
                    this.Arguments.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
