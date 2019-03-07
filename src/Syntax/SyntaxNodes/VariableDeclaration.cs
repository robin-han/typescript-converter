using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class VariableDeclarationNode : Declaration
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VariableDeclaration; }
        }

        public Node Name
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
                    this._type = this.InferType();
                }
                return this._type;
            }
            private set
            {
                this._type = value;
            }
        }

        public Node Initializer
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Name = null;
            this.Initializer = null;

            this.Type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "name":
                    this.Name = childNode;
                    break;

                case "type":
                    this.Type = childNode;
                    break;

                case "initializer":
                    this.Initializer = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            if (this.Parent.Kind == NodeKind.CatchClause)
            {
                return this.CreateNode(NodeKind.Identifier, "Exception");
            }

            Node type = null;
            Node initValue = this.Initializer;
            if (initValue != null)
            {
                type = this.GetNodeType(initValue);
            }
            return type ?? this.CreateNode(NodeKind.AnyKeyword);
        }

    }
}

