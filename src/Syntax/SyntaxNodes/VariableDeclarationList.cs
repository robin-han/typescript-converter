using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class VariableDeclarationList : Node
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.VariableDeclarationList; }
        }

        public List<Node> Declarations
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
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Declarations = new List<Node>();
            this._type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "declarations":
                    this.Declarations.Add(childNode);
                    break;

                case "type":
                    this._type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            VariableDeclarationNode variableDeclaration = this.Declarations.Count > 0 ? this.Declarations[0] as VariableDeclarationNode : null;
            Node variableType = variableDeclaration == null ? null : variableDeclaration.Type;

            if (variableType == null)
            {
                variableType = this.CreateNode(NodeKind.AnyKeyword);
            }
            return variableType;
        }

    }
}

