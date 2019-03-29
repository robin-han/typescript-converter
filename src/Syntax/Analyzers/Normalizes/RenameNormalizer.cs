﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class RenameNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.SetAccessor:
                    this.RenameParameterName(node as SetAccessor);
                    break;

                default:
                    break;
            }
        }

        private void RenameParameterName(SetAccessor setAccessor)
        {
            string paramName = setAccessor.Parameters[0].Name.Text;
            if (paramName != "value")
            {
                List<Node> referencedNode = setAccessor.Body.Descendants(n => n.Kind == NodeKind.Identifier && n.Text == paramName);
                foreach (Node node in referencedNode)
                {
                    node.Text = "value";
                }
            }
        }
    }
}
