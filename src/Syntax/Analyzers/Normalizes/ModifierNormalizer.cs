using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class ModifierNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.MethodDeclaration:
                case NodeKind.GetAccessor:
                case NodeKind.SetAccessor:
                case NodeKind.PropertyDeclaration:
                case NodeKind.Constructor:
                    this.NormalizeModify(node);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeModify(Node node)
        {
            List<Node> modifiers = node.GetValue("Modifiers") as List<Node>;
            if (modifiers == null)
            {
                return;
            }

            if (!node.HasModify(NodeKind.PublicKeyword) && !node.HasModify(NodeKind.PrivateKeyword) && !node.HasModify(NodeKind.ProtectedKeyword))
            {
                modifiers.Add(NodeHelper.CreateNode(NodeKind.PublicKeyword));
            }

            if (node.HasJsDocTag("csoverride") && !node.HasModify(NodeKind.OverrideKeyword))
            {
                modifiers.Add(NodeHelper.CreateNode(NodeKind.OverrideKeyword));
            }
            if (node.HasJsDocTag("csnew") && !node.HasModify(NodeKind.NewKeyword))
            {
                modifiers.Add(NodeHelper.CreateNode(NodeKind.NewKeyword));
            }
        }


    }
}
