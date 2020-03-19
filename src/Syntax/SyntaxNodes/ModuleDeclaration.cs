using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TypeScript.Syntax
{
    public class ModuleDeclaration : Declaration
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ModuleDeclaration; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public Node Body
        {
            get;
            private set;
        }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Name = null;
            this.Body = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "body":
                    this.Body = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }


        public ModuleBlock GetModuleBlock()
        {
            ModuleDeclaration md = this;
            while (md != null)
            {
                if (md.Body.Kind == NodeKind.ModuleBlock)
                {
                    return md.Body as ModuleBlock;
                }
                md = md.Body as ModuleDeclaration;
            }
            return null;
        }

    }
}

