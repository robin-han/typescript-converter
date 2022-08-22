using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Analysis
{
    public class UnnessaryNodeNormalizer : Normalizer
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
                case NodeKind.ModuleBlock:
                    this.RemoveModuleBlockStatments(node as ModuleBlock);
                    break;

                case NodeKind.ClassDeclaration:
                    this.RemoveClassMembers(node as ClassDeclaration);
                    break;

                default:
                    break;
            }
        }

        #region ModuleBlock
        private void RemoveModuleBlockStatments(ModuleBlock module)
        {
            for (int i = module.Statements.Count - 1; i >= 0; i--)
            {
                Node statement = module.Statements[i];
                switch (statement.Kind)
                {
                    case NodeKind.ExpressionStatement:
                        if (statement.Text.IndexOf("use strict") >= 0) // 'use strict', ;)
                        {
                            module.Statements.RemoveAt(i);
                        }
                        break;

                    case NodeKind.FunctionDeclaration:
                    case NodeKind.ClassDeclaration:
                        if (statement.HasJsDocTag("csignore"))
                        {
                            module.Statements.RemoveAt(i);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion

        private void RemoveClassMembers(ClassDeclaration classNode)
        {
            classNode.RemoveAllMembers(member =>
            {
                switch (member.Kind)
                {
                    case NodeKind.SemicolonClassElement: //;
                    case NodeKind.IndexSignature: //ley
                        return true;

                    default:
                        return false;
                }
            });
        }
    }

}

