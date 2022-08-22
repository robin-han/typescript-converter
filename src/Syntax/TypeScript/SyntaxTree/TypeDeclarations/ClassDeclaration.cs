using System;
using System.Collections.Generic;

namespace TypeScript.Syntax
{
    [NodeKindAttribute(NodeKind.ClassDeclaration)]
    public class ClassDeclaration : TypeDeclaration
    {
        public ClassDeclaration()
        {
            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.TypeParameters = new List<Node>();
            this.HeritageClauses = new List<Node>();
            this.Members = new List<Node>();
            this.Decorators = new List<Node>();
        }

        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ClassDeclaration; }
        }

        public virtual List<Node> Members
        {
            get;
            protected set;
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public List<Node> Decorators
        {
            get;
            private set;
        }

        public string NameText
        {
            get
            {
                return (this.Name != null ? this.Name.Text : string.Empty);
            }
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public List<Node> HeritageClauses
        {
            get;
            private set;
        }

        public bool IsExport
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ExportKeyword);
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.DefaultKeyword);
            }
        }

        public Node Extending
        {
            get
            {
                foreach (HeritageClause heritage in this.HeritageClauses)
                {
                    if (heritage.Text.Contains("extends"))
                    {
                        return heritage.Types[0];
                    }
                }
                return null;
            }
        }

        public List<Node> Implementing
        {
            get
            {
                List<Node> ret = new List<Node>();
                foreach (HeritageClause heritage in this.HeritageClauses)
                {
                    if (heritage.Text.Contains("implements"))
                    {
                        ret.AddRange(heritage.Types);
                    }
                }
                return ret;
            }
        }
        #endregion

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "heritageClauses":
                    this.HeritageClauses.Add(childNode);
                    break;

                case "members":
                    this.Members.Add(childNode);
                    break;

                case "decorators":
                    this.Decorators.Add(childNode);
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        public Constructor GetConstructor()
        {
            return this.Members.Find(m => m.Kind == NodeKind.Constructor) as Constructor;
        }

        /// <summary>
        /// Gets defined inherit constructor
        /// </summary>
        /// <returns></returns>
        public Constructor GetInheritConstructor()
        {
            List<ClassDeclaration> baseClasses = this.Document.Project.GetInheritClasses(this);
            foreach (var baseClass in baseClasses)
            {
                Constructor ctor = baseClass.GetConstructor();
                if (ctor != null && ctor.Parameters.Count > 0)
                {
                    return ctor;
                }
            }
            return null;
        }

        public List<Node> GetMembers(string name)
        {
            List<Node> ret = new List<Node>();

            foreach (Node member in this.Members)
            {
                switch (member.Kind)
                {
                    case NodeKind.MethodDeclaration:
                        MethodDeclaration method = member as MethodDeclaration;
                        if (method.Name.Text == name)
                        {
                            ret.Add(method);
                        }
                        break;
                    case NodeKind.GetAccessor:
                        GetAccessor getAccess = member as GetAccessor;
                        if (getAccess.Name.Text == name)
                        {
                            ret.Add(getAccess);
                        }
                        break;
                    case NodeKind.SetAccessor:
                        SetAccessor setAccess = member as SetAccessor;
                        if (setAccess.Name.Text == name)
                        {
                            ret.Add(setAccess);
                        }
                        break;
                    case NodeKind.GetSetAccessor:
                        GetSetAccessor getSetAccess = member as GetSetAccessor;
                        if (getSetAccess.Name.Text == name)
                        {
                            ret.Add(getSetAccess);
                        }
                        break;
                    case NodeKind.PropertyDeclaration:
                        PropertyDeclaration prop = member as PropertyDeclaration;
                        if (prop.Name.Text == name)
                        {
                            ret.Add(prop);
                        }
                        break;
                    default:
                        break;
                }
            }

            return ret;
        }

        public void AddMember(Node member, bool changeParent = true)
        {
            if (changeParent)
            {
                member.Parent = this;
            }
            this.Members.Add(member);
        }

        public void InsertMember(int index, Node member, bool changeParent = true)
        {
            if (changeParent)
            {
                member.Parent = this;
            }
            this.Members.Insert(index, member);
        }

        public void InsertMembers(int index, List<Node> members, bool changeParent = true)
        {
            if (changeParent)
            {
                foreach (var mem in members)
                {
                    mem.Parent = this;
                }
            }
            this.Members.InsertRange(index, members);
        }

        public void RemoveMemberAt(int index)
        {
            this.Members.RemoveAt(index);
        }

        public int RemoveAllMembers(Predicate<Node> match)
        {
            return this.Members.RemoveAll(match);
        }

        /// <summary>
        /// Get filed include inherit
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyDeclaration GetField(string name)
        {
            PropertyDeclaration field = null;
            field = this.Members.Find(m => (m is PropertyDeclaration p && p.Name.Text == name)) as PropertyDeclaration;
            if (field != null)
            {
                return field;
            }

            foreach (ClassDeclaration clazz in this.Document.Project.GetInheritClasses(this))
            {
                field = clazz.Members.Find(m => (m is PropertyDeclaration p && p.Name.Text == name)) as PropertyDeclaration;
                if (field != null)
                {
                    return field;
                }
            }
            return null;
        }
    }
}
