using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class CombineNodeNormalizer : Normalizer
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
                case NodeKind.ClassDeclaration:
                    this.CombineGetSetAccess(node as ClassDeclaration);
                    break;

                default:
                    break;
            }
        }

        #region ClassDeclaration
        private void CombineGetSetAccess(ClassDeclaration classNode)
        {
            List<Node> removedNodes = new List<Node>();

            for (int i = 0; i < classNode.Members.Count; i++)
            {
                if (classNode.Members[i].Kind != NodeKind.GetAccessor)
                {
                    continue;
                }

                GetAccessor getAccessor = classNode.Members[i] as GetAccessor;
                SetAccessor setAccessor = classNode.Members.Find(c =>
                    (c.Kind == NodeKind.SetAccessor) &&
                    ((c as SetAccessor).Name.Text == getAccessor.Name.Text)) as SetAccessor;

                if (setAccessor != null)
                {
                    removedNodes.Add(getAccessor);
                    removedNodes.Add(setAccessor);

                    Node getSestAccessor = NodeHelper.CreateNode(NodeKind.GetSetAccessor);
                    getSestAccessor.Parent = classNode;
                    getSestAccessor.AddChild(getAccessor.TsNode);
                    getSestAccessor.AddChild(setAccessor.TsNode);
                    classNode.InsertMember(i++, getSestAccessor);
                }
            }

            classNode.RemoveAllMembers(m => removedNodes.IndexOf(m) >= 0);
        }
        #endregion
    }
}
