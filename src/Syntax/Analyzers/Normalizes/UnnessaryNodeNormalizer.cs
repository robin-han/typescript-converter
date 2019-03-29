using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax.Analysis
{
    public class UnnessaryNodeNormalizer : Normalizer
    {
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

                    case NodeKind.TypeAliasDeclaration:
                        TypeAliasDeclaration alias = statement as TypeAliasDeclaration;
                        if (alias.Type.Kind != NodeKind.FunctionType)
                        {
                            module.Statements.RemoveAt(i);
                            module.TypeAliases.Add(statement);
                        }
                        break;

                    case NodeKind.FunctionDeclaration:
                    case NodeKind.ClassDeclaration:
                        Declaration declaration = statement as Declaration;
                        if (declaration.HasJsDocTag("csignore"))
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
            for (int i = 0; i < classNode.Members.Count; i++)
            {
                Node member = classNode.Members[i];

                switch (member.Kind)
                {
                    case NodeKind.SemicolonClassElement: //;
                    case NodeKind.IndexSignature: //ley
                        classNode.Members.RemoveAt(i--);
                        break;

                    default:
                        break;
                }
            }
        }
    }

}

