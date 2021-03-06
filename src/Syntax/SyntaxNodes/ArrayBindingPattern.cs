using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ArrayBindingPattern : Node
    {
        //export interface ArrayBindingPattern extends Node
        //{
        //    kind: SyntaxKind.ArrayBindingPattern;
        //    parent: VariableDeclaration | ParameterDeclaration | BindingElement;
        //    elements: NodeArray<ArrayBindingElement>;
        //}

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ArrayBindingPattern; }
        }

        public List<Node> Elements
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Elements = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "elements":
                    this.Elements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

    }
}

