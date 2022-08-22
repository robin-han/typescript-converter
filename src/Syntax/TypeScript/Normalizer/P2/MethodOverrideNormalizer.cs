using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeScript.Syntax.Analysis
{
    public class MethodOverrideNormalizer : Normalizer
    {
        public override int Priority
        {
            get { return 210; }
        }

        protected override void Visit(Node node)
        {
            base.Visit(node);

            switch (node.Kind)
            {
                case NodeKind.MethodDeclaration:
                case NodeKind.GetAccessor:
                case NodeKind.PropertyDeclaration:
                    this.NormalizeModify(node);
                    break;

                default:
                    break;
            }
        }

        private void NormalizeModify(Node method)
        {
            if (method.Kind == NodeKind.MethodDeclaration && HasJsDocTag("virtual", ((MethodDeclaration)method).JsDoc))
            {
                method.AddModify(NodeKind.VirtualKeyword);
            }

            if (method.HasModify(NodeKind.AbstractKeyword) ||
            method.HasModify(NodeKind.PrivateKeyword) ||
            //method.HasModify(NodeKind.StaticKeyword) ||
            method.HasModify(NodeKind.NewKeyword) ||
            method.HasModify(NodeKind.OverrideKeyword))
            {
                return;
            }

            if (!(method.Parent is ClassDeclaration classNode) || (classNode.HeritageClauses.Count == 0))
            {
                return;
            }

            Node baseMethod = this.GetBaseMethod(classNode, method);
            if (baseMethod == null)
            {
                return;
            }

            if (method.HasModify(NodeKind.StaticKeyword)) //static add new
            {
                if (IsSameParameters(method, baseMethod))
                {
                    method.AddModify(NodeKind.NewKeyword);
                }
                return;
            }

            bool sameSignature = this.IsSameSignature(method, baseMethod);
            Node baseClass = baseMethod.Parent;
            if (baseMethod.HasModify(NodeKind.AbstractKeyword) && !sameSignature)
            {
                //Error: Must has same signature with its base
                method.AddModify(NodeKind.OverrideKeyword);
            }
            else if (baseClass.Kind == NodeKind.ClassDeclaration)
            {
                if (sameSignature)
                {
                    if (!baseMethod.HasModify(NodeKind.AbstractKeyword) && !baseMethod.HasModify(NodeKind.OverrideKeyword))
                    {
                        baseMethod.AddModify(NodeKind.VirtualKeyword);
                    }
                    if (!method.HasModify(NodeKind.OverrideKeyword))
                    {
                        method.RemoveModify(NodeKind.VirtualKeyword);
                        method.AddModify(NodeKind.OverrideKeyword);
                    }
                }
                else
                {
                    if (this.IsSameParameters(method, baseMethod) && this.IsSameTypeParameters(method, baseMethod) && !method.HasModify(NodeKind.NewKeyword))
                    {
                        method.AddModify(NodeKind.NewKeyword);
                    }
                }
            }
        }

        private bool HasJsDocTag(string tagName, List<Node> jsDoc)
        {
            if (jsDoc.Count > 0 && jsDoc[0] is JSDocComment jsDocComment)
            {
                foreach (var tag in jsDocComment.Tags)
                {
                    if (tag.Kind == NodeKind.JSDocTag)
                    {
                        JSDocTag docTag = (JSDocTag)tag;
                        if (docTag.TagName.Text == tagName)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Node GetBaseMethod(ClassDeclaration classNode, Node method)
        {
            Project project = classNode.Document?.Project;
            if (project == null)
            {
                return null;
            }

            Node methodNode = method.GetValue("Name") as Node;
            string methodName = methodNode?.Text;
            if (string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            Node baseMethod = null;
            List<Node> baseTypes = project.GetInherits(classNode);
            foreach (Node baseType in baseTypes)
            {
                switch (baseType.Kind)
                {
                    case NodeKind.ClassDeclaration:
                        List<Node> baseMethods = (baseType as ClassDeclaration).GetMembers(methodName);
                        if (baseMethods.Count == 0)
                        {
                            break;
                        }

                        if (method.Kind == NodeKind.MethodDeclaration)
                        {
                            Node mostLike = baseMethods.Find(
                                b => b.Kind == method.Kind &&
                                (b as MethodDeclaration).Parameters.Count == (method as MethodDeclaration).Parameters.Count);
                            if (mostLike != null)
                            {
                                return mostLike;
                            }

                            if (baseMethod == null)
                            {
                                baseMethod = baseMethods[0];
                            }
                        }
                        else
                        {
                            return baseMethods[0];
                        }
                        break;

                    default:
                        break;
                }
            }

            return baseMethod;
        }

        private bool IsSameSignature(Node method, Node baseMethod)
        {
            if (!this.IsSameName(method, baseMethod))
            {
                return false;
            }
            if (!this.IsSameType(method, baseMethod))
            {
                return false;
            }

            if (method.Kind != NodeKind.PropertyDeclaration && baseMethod.Kind != NodeKind.PropertyDeclaration)
            {
                if (!IsSameTypeParameters(method, baseMethod))
                {
                    return false;
                }

                if (!baseMethod.HasModify(NodeKind.AbstractKeyword) && !IsSameParameters(method, baseMethod))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSameName(Node method1, Node method2)
        {
            Node name1 = method1.GetValue("Name") as Node;
            Node name2 = method2.GetValue("Name") as Node;
            if (name1 == null && name2 == null)
            {
                return true;
            }
            if (name1 == null || name2 == null)
            {
                return false;
            }

            //
            return name1.Text == name2.Text;
        }

        private bool IsSameType(Node node1, Node node2)
        {
            Node type1 = node1.GetValue("Type") as Node;
            Node type2 = node2.GetValue("Type") as Node;

            return TypeHelper.IsSameType(type1, type2);
        }

        private bool IsSameTypeParameters(Node method1, Node method2)
        {
            List<Node> typeParams1 = method1.GetValue("TypeParameters") as List<Node>;
            List<Node> typeParams2 = method2.GetValue("TypeParameters") as List<Node>;
            if (typeParams1 == null && typeParams2 == null)
            {
                return true;
            }
            if (typeParams1 == null || typeParams2 == null)
            {
                return false;
            }
            //
            if (typeParams1.Count != typeParams2.Count)
            {
                return false;
            }
            for (int i = 0; i < typeParams1.Count; i++)
            {
                if (typeParams1[i].Text != typeParams2[i].Text)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSameParameters(Node method1, Node method2)
        {
            List<Node> params1 = method1.GetValue("Parameters") as List<Node>;
            List<Node> params2 = method2.GetValue("Parameters") as List<Node>;
            if (params1 == null && params2 == null)
            {
                return true;
            }
            if (params1 == null || params2 == null)
            {
                return false;
            }
            //
            if (params1.Count != params2.Count)
            {
                return false;
            }
            for (int i = 0; i < params1.Count; i++)
            {
                Parameter p1 = params1[i] as Parameter;
                Parameter p2 = params2[i] as Parameter;
                if (!this.IsSameType(p1, p2))
                {
                    return false;
                }
                if ((p1.DotDotDotToken != null) != (p2.DotDotDotToken != null))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
