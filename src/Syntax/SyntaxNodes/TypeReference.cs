using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class TypeReference : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.TypeReference; }
        }

        public Node TypeName
        {
            get;
            private set;
        }

        public List<Node> TypeArguments
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.TypeName = null;
            this.TypeArguments = new List<Node>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "typeName":
                    this.TypeName = childNode;
                    break;

                case "typeArguments":
                    this.TypeArguments.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            this.Text = this.TypeName.Text;
        }

    }
}

