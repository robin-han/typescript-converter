using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ForOfStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForOfStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }

        public Node Identifier
        {
            get
            {
                VariableDeclarationNode variableDeclaration = this.GetVariableDeclaration();
                if (variableDeclaration != null)
                {
                    return variableDeclaration.Name;
                }
                return null;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Initializer = null;
            this.Expression = null;
            this.Statement = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        private VariableDeclarationNode GetVariableDeclaration()
        {
            VariableDeclarationList initializer = this.Initializer as VariableDeclarationList;
            if (initializer != null && initializer.Declarations.Count > 0)
            {
                return initializer.Declarations[0] as VariableDeclarationNode;
            }
            return null;
        }
    }
}

