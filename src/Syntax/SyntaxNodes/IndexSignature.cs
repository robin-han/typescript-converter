using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class IndexSignature : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.IndexSignature; }
        }

        public List<Node> Parameters
        {
            get;
            private set;
        }

        public Node Type
        {
            get;
            internal set;
        }

        public Node KeyType
        {
            get
            {
                if (Parameters.Count == 0)
                {
                    return NodeHelper.CreateNode(NodeKind.StringKeyword);
                }
                return (this.Parameters[0] as Parameter).Type;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Parameters = new List<Node>();
            this.Type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "parameters":
                    this.Parameters.Add(childNode);
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

