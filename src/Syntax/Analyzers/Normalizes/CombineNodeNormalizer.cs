using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class CombineNodeNormalizer : Normalizer
    {
        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.ModuleDeclaration:
                    this.CombineModules(node as ModuleDeclaration);
                    break;

                case NodeKind.ClassDeclaration:
                    this.CombineGetSetAccess(node as ClassDeclaration);
                    break;

                default:
                    break;
            }
        }

        #region ModuleDeclaration
        private void CombineModules(ModuleDeclaration module)
        {
            List<string> mTexts = new List<string>();
            while (true)
            {
                mTexts.Add(module.Name.Text);
                if (module.Body == null || module.Body.Kind != NodeKind.ModuleDeclaration)
                {
                    break;
                }
                module = module.Body as ModuleDeclaration;
            }

            if (mTexts.Count > 1)
            {
                module.Name.Text = string.Join('.', mTexts);
                module.Name.End = module.Name.End;

                module.Body = module.Body;
            }
        }
        #endregion

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

                    Node getSestAccessor = classNode.CreateNode(NodeKind.GetSetAccessor);
                    getSestAccessor.AddNode(getAccessor.TsNode);
                    getSestAccessor.AddNode(setAccessor.TsNode);
                    classNode.Members.Insert(i++, getSestAccessor);
                }
            }

            classNode.Members.RemoveAll(m => removedNodes.IndexOf(m) >= 0);
        }
        #endregion
    }
}
