using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class ModuleBlock : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ModuleBlock; }
        }

        public List<Node> Statements
        {
            get;
            private set;
        }

        public List<Node> TypeAliases
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Statements = new List<Node>();

            this.TypeAliases = new List<Node>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "statements":
                    this.Statements.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        protected override void NormalizeImp()
        {
            ClassifyStatments();
        }

        private void ClassifyStatments()
        {
            for (int i = 0; i < this.Statements.Count; i++)
            {
                Node statement = this.Statements[i];
                if (statement.Kind == NodeKind.ExpressionStatement && statement.Text.IndexOf("use strict") >= 0) // 'use strict', ;
                {
                    this.Statements.RemoveAt(i--);
                }
                else if (statement.Kind == NodeKind.TypeAliasDeclaration)
                {
                    TypeAliasDeclaration alias = statement as TypeAliasDeclaration;
                    if (alias.Type.Kind != NodeKind.FunctionType)
                    {
                        this.Statements.RemoveAt(i--);
                        this.TypeAliases.Add(statement);
                    }
                }
            }
        }

    }
}

