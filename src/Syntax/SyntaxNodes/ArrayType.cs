using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ArrayType : Node
    {
        private Node _elementType = null;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayType; }
        }

        public Node ElementType
        {
            get
            {
                return this._elementType;
            }
            internal set
            {
                this._elementType = value;
                if (this._elementType != null)
                {
                    this._elementType.Parent = this;
                }
            }
        }

        public bool IsParams
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
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

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

