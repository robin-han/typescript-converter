using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
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

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

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

        protected override void NormalizeImp()
        {
            base.NormalizeImp();

            this.CombineModules();
        }

        private void CombineModules()
        {
            List<string> mTexts = new List<string>();
            ModuleDeclaration node = this;
            while (true)
            {
                mTexts.Add(node.Name.Text);
                if (node.Body == null || node.Body.Kind != NodeKind.ModuleDeclaration)
                {
                    break;
                }
                node = node.Body as ModuleDeclaration;
            }

            if (mTexts.Count > 1)
            {
                this.Name.Text = string.Join('.', mTexts);
                this.Name.End = node.Name.End;

                this.Body = node.Body;
            }
        }

    }
}

