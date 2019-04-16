using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ArrayType : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayType; }
        }

        public Node ElementType
        {
            get;
            private set;
        }

        public bool IsParams
        {
            get;
            set;
        }

        public bool IsEnumerable
        {
            get;
            set;
        }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.ElementType = null;

            this.IsParams = false;
            this.IsEnumerable = false;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "elementType":
                    this.ElementType = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

