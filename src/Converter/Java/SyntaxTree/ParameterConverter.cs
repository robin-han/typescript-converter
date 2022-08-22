using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;
using com.sun.tools.javac.code;

namespace TypeScript.Converter.Java
{
    public class ParameterConverter : NodeConverter
    {
        public JCTree Convert(Parameter node)
        {
            JCExpression type = node.Type?.ToJavaSyntaxTree<JCExpression>();
            Name name = Names.fromString(node.Name.Text);
            JCModifiers modifiers = TreeMaker.Modifiers(0);
            if (node.IsVariable)
            {
                modifiers = TreeMaker.Modifiers(Flags.VARARGS);
                type = TreeMaker.TypeArray(node.VariableType.ToJavaSyntaxTree<JCExpression>());
            }

            if (ShouldAddFinalModifier(node))
            {
                modifiers = TreeMaker.Modifiers(modifiers.flags | (long)Flags.FINAL);
            }

            return TreeMaker.VarDef(modifiers, name, type, null);
        }

        /// <summary>
        ///  Parameter should add final if descend arrow function ref it.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ShouldAddFinalModifier(Parameter node)
        {
            Node methodBody = null;
            if (node.Parent.Kind == NodeKind.MethodDeclaration)
            {
                methodBody = ((MethodDeclaration)node.Parent).Body;
            }
            else if (node.Parent.Kind == NodeKind.Constructor)
            {
                methodBody = ((Constructor)node.Parent).Body;
            }
            else if (node.Parent.Kind == NodeKind.ArrowFunction)
            {
                methodBody = ((ArrowFunction)node.Parent).Body;
            }

            if (methodBody != null)
            {
                string name = node.Name.Text;
                List<Node> arrowFunctions = methodBody.Descendants(d => d.Kind == NodeKind.ArrowFunction);
                foreach (ArrowFunction arrow in arrowFunctions)
                {
                    List<Node> refs = arrow.Body.DescendantsAndSelfOnce(d =>
                    {
                        if (d.Kind == NodeKind.Identifier && d.Text == name)
                        {
                            if (d.Parent.Kind == NodeKind.PropertyAccessExpression)
                            {
                                return d == ((PropertyAccessExpression)d.Parent).Expression;
                            }
                            return true;
                        }
                        return false;
                    });
                    if (refs.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

