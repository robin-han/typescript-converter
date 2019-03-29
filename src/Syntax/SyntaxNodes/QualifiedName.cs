using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class QualifiedName : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.QualifiedName; }
        }

        public Node Left
        {
            get;
            private set;
        }

        public Node Right
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Left = null;
            this.Right = null;
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "left":
                    this.Left = childNode;
                    break;

                case "right":
                    this.Right = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

