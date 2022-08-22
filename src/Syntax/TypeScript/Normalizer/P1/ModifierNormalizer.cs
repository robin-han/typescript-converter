using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class ModifierNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 10; }
        }

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
                    NormalizeModify(node);
                    break;

                default:
                    break;
            }
        }

        public static void NormalizeModify(Node node)
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
        }
    }
}
