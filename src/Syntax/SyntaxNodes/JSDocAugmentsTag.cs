using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class JSDocAugmentsTag : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocAugmentsTag; }
        }

        public Node AtToken
        {
            get;
            private set;
        }

        public Node TagName
        {
            get;
            private set;
        }

        public Node Class
        {
            get;
            private set;
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.AtToken = null;
            this.TagName = null;
            this.Class = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "atToken":
                    this.AtToken = childNode;
                    break;

                case "tagName":
                    this.TagName = childNode;
                    break;

                case "class":
                    this.Class = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

