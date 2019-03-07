using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ArrayType : Node
    {
        private bool _isParams = false;

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
            get
            {
                return this._isParams;
            }
            set
            {
                this._isParams = value;
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.ElementType = null;
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

