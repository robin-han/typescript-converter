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

            if (!this.HasModify(modifiers, NodeKind.PublicKeyword) && !this.HasModify(modifiers, NodeKind.PrivateKeyword) && !this.HasModify(modifiers, NodeKind.ProtectedKeyword))
            {
                modifiers.Add(node.CreateNode(NodeKind.PublicKeyword));
            }

            if (node.HasJsDocTag("csoverride") && !this.HasModify(modifiers, NodeKind.OverrideKeyword))
            {
                modifiers.Add(node.CreateNode(NodeKind.OverrideKeyword));
            }
            if (node.HasJsDocTag("csnew") && !this.HasModify(modifiers, NodeKind.NewKeyword))
            {
                modifiers.Add(node.CreateNode(NodeKind.NewKeyword));
            }
        }

        private bool HasModify(List<Node> modifiers, NodeKind modify)
        {
            return modifiers.Exists(n => n.Kind == modify);
        }
    }
}
