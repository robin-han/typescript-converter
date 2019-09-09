using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class VariableDeclarationList : Node
    {
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
            get;
            internal set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Declarations = new List<Node>();
            this.Type = null;
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
                    this.Type = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

