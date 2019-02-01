using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class GetSetAccessor : Node
    {
        private Node _type;

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.GetSetAccessor; }
        }

        public GetAccessor GetAccessor
        {
            get;
            private set;
        }

        public SetAccessor SetAccessor
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get
            {
                var accessor = this.GetAccessor;
                if (accessor != null)
                {
                    return accessor.Modifiers;
                }
                return new List<Node>();
            }
        }

        public List<Node> JsDoc
        {
            get
            {
                var accessor = this.GetAccessor;
                if (accessor != null)
                {
                    return accessor.JsDoc;
                }
                return new List<Node>();
            }
        }

        public Node Name
        {
            get
            {
                var accessor = this.GetAccessor;
                if (accessor != null)
                {
                    return accessor.Name;
                }
                return null;
            }
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

            this.GetAccessor = null;
            this.SetAccessor = null;
            this._type = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            NodeKind kind = childNode.Kind;
            switch (kind)
            {
                case NodeKind.GetAccessor:
                    this.GetAccessor = childNode as GetAccessor;
                    break;

                case NodeKind.SetAccessor:
                    this.SetAccessor = childNode as SetAccessor;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override Node InferType()
        {
            return this.GetAccessor.Type != null ? this.GetAccessor.Type : this.SetAccessor.Parameters[0].Type;
        }

    }
}

