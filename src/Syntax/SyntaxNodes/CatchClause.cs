using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class CatchClause : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.CatchClause; }
        }

        public VariableDeclarationNode VariableDeclaration
        {
            get;
            private set;
        }

        public Node Block
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.VariableDeclaration = null;
            this.Block = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "variableDeclaration":
                    this.VariableDeclaration = childNode as VariableDeclarationNode;
                    break;

                case "block":
                    this.Block = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

