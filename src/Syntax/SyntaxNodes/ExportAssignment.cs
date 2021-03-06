﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ExportAssignment : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ExportAssignment; }
        }

        public Node Expression
        {
            get;
            private set;
        }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "expression":
                    this.Expression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public Node GetTypeDefinition() // export default AA;
        {
            string typeName = this.Expression.Text;

            SourceFile sourceFile = this.Ancestor(NodeKind.SourceFile) as SourceFile;
            Node definition = sourceFile.GetOwnModuleTypeDefinition(typeName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDefinition(typeName);
            if (definition != null)
            {
                return definition;
            }

            return null;
        }
    }
}

