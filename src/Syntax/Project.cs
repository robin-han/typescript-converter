using TypeScript.Syntax.Analysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeScript.Syntax
{
    public class Project
    {
        #region Fields
        private List<Document> _allDocuments;
        private List<Document> _documents;
        private (List<Node> nodes, List<string> names)? _typeNodes;
        private List<Type> _analyzerTypes;
        #endregion

        #region Constructor
        public Project(List<Document> allDocs, List<Document> docs)
        {
            this._allDocuments = allDocs;
            this._documents = docs;
            this._typeNodes = null;

            foreach (Document doc in allDocs)
            {
                doc.Project = this;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets all documents.
        /// </summary>
        public List<Document> AllDocuments
        {
            get
            {
                return this._allDocuments;
            }
        }

        /// <summary>
        /// Gets convert documents.
        /// </summary>
        public List<Document> Documents
        {
            get
            {
                return this._documents;
            }
        }

        /// <summary>
        ///  Gets all the type names in the project.
        /// </summary>
        public List<string> TypeNames
        {
            get
            {
                return this.GetAllTypes().names;
            }
        }

        /// <summary>
        /// Gets all types in the project.
        /// </summary>
        public List<Node> Types
        {
            get
            {
                return this.GetAllTypes().nodes;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add document to document collection
        /// </summary>
        /// <param name="doc"></param>
        public void AddDocument(Document doc)
        {
            if (!this.Documents.Contains(doc))
            {
                this.Documents.Add(doc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Document GetDocument(string typeName)
        {
            int index = this.TypeNames.IndexOf(typeName);
            if (index >= 0)
            {
                return this.Types[index].Document;
            }
            return null;
        }

        /// <summary>
        /// Gets class declaration node with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ClassDeclaration GetClass(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (Node type in this.Types)
            {
                if (type.Kind == NodeKind.ClassDeclaration)
                {
                    ClassDeclaration cls = type as ClassDeclaration;
                    if (cls.Name != null && cls.Name.Text == name)
                    {
                        return cls;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets interface declaration node with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InterfaceDeclaration GetInterface(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (Node type in this.Types)
            {
                if (type.Kind == NodeKind.InterfaceDeclaration)
                {
                    InterfaceDeclaration @interface = type as InterfaceDeclaration;
                    if (@interface.Name != null && @interface.Name.Text == name)
                    {
                        return @interface;
                    }
                }
            }
            return null;
        }

        public ClassDeclaration GetBaseClass(ClassDeclaration classNode)
        {
            if (classNode == null)
            {
                return null;
            }

            foreach (HeritageClause inherit in classNode.HeritageClauses)
            {
                if (inherit.Token == NodeKind.ExtendsKeyword && inherit.Types.Count > 0)
                {
                    return this.GetClass(TypeHelper.GetTypeName(inherit.Types[0]));
                }
            }
            return null;
        }

        public List<Node> GetInherits(Node node)
        {
            List<Node> ret = new List<Node>();

            Queue<HeritageClause> heritages = new Queue<HeritageClause>();
            if (node.Kind == NodeKind.ClassDeclaration)
            {
                foreach (HeritageClause item in (node as ClassDeclaration).HeritageClauses)
                {
                    heritages.Enqueue(item);
                }
            }
            else if (node.Kind == NodeKind.InterfaceDeclaration)
            {
                foreach (HeritageClause item in (node as InterfaceDeclaration).HeritageClauses)
                {
                    heritages.Enqueue(item);
                }
            }
            while (heritages.Count > 0)
            {
                HeritageClause inherit = heritages.Dequeue();
                foreach (Node type in inherit.Types)
                {
                    if (inherit.Token == NodeKind.ExtendsKeyword)
                    {
                        ClassDeclaration baseClass = this.GetClass(TypeHelper.GetTypeName(type));
                        if (baseClass != null)
                        {
                            ret.Add(baseClass);
                            foreach (HeritageClause nextInherit in baseClass.HeritageClauses)
                            {
                                heritages.Enqueue(nextInherit);
                            }
                        }
                    }
                    else
                    {
                        InterfaceDeclaration baseInterface = this.GetInterface(TypeHelper.GetTypeName(type));
                        if (baseInterface != null)
                        {
                            ret.Add(baseInterface);
                            foreach (HeritageClause nextInherit in baseInterface.HeritageClauses)
                            {
                                heritages.Enqueue(nextInherit);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets all reference type(class, interface, enums etc.) names.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public List<string> GetReferences(params string[] typeNames)
        {
            List<string> ret = new List<string>();
            foreach (string name in typeNames)
            {
                ret.Add(name);
                this.GetReferences(name, ret);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="results"></param>
        private void GetReferences(string typeName, List<string> result)
        {
            var (nodes, names) = this.GetAllTypes();
            int index = names.IndexOf(typeName);
            if (index == -1)
            {
                return;
            }

            Node rootNode = nodes[index];
            if (rootNode.Document != null && !this.Documents.Contains(rootNode.Document))
            {
                return;
            }

            List<Node> types = new List<Node>();
            rootNode.Descendants(n =>
            {
                if (n.GetValue("Type") is Node t)
                {
                    types.Add(t);
                }
                if (n.GetValue("Types") is List<Node> ts)
                {
                    types.AddRange(ts);
                }

                switch (n.Kind)
                {
                    case NodeKind.PropertyAccessExpression:
                        PropertyAccessExpression access = n as PropertyAccessExpression;

                        if (access.Expression.Kind == NodeKind.Identifier && Regex.IsMatch(access.Expression.Text, "^[_A-Z]+[_A-Za-z0-9]*$") && names.Contains(access.Expression.Text))
                        {
                            types.Add(access.Expression);
                        }
                        else if (access.Name.Kind == NodeKind.Identifier && Regex.IsMatch(access.Name.Text, "^[_A-Z]+[_A-Za-z0-9]*$") && names.Contains(access.Name.Text))
                        {
                            types.Add(access.Name);
                        }
                        break;

                    case NodeKind.BinaryExpression:
                        BinaryExpression binary = n as BinaryExpression;
                        if (binary.OperatorToken.Kind == NodeKind.InstanceOfKeyword)
                        {
                            types.Add(binary.Right);
                        }
                        break;

                    default:
                        break;
                }
                return false;
            });

            for (int i = 0; i < types.Count; i++)
            {
                Node type = types[i];
                if (type.Kind == NodeKind.ArrayType)
                {
                    type = (type as ArrayType).ElementType;
                }
                string[] parts = type.Text.Split('.');
                string name = parts[parts.Length - 1].Trim();
                if (Regex.IsMatch(name, "^[_A-Za-z]+[_A-Za-z0-9]*$") && !result.Contains(name) && names.Contains(name))
                {
                    result.Add(name);
                    this.GetReferences(name, result);
                }
            }
        }

        /// <summary>
        /// Get all type nodes(class, interfact, enum etc.) in the project.
        /// </summary>
        /// <returns></returns>
        private (List<Node> nodes, List<string> names) GetAllTypes()
        {
            if (this._typeNodes.HasValue)
            {
                return (this._typeNodes.Value.nodes, this._typeNodes.Value.names);
            }

            //
            List<Node> nodes = new List<Node>();
            List<string> names = new List<string>();
            foreach (Document doc in this.AllDocuments)
            {
                List<Node> docTypes = doc.GetTypeNodes();
                foreach (Node node in docTypes)
                {
                    string name = doc.GetTypeName(node);
                    if (!string.IsNullOrEmpty(name) && !names.Contains(name))
                    {
                        nodes.Add(node);
                        names.Add(name);
                    }
                }
            }

            this._typeNodes = (nodes, names);
            return (nodes, names);
        }

        public List<Type> GetAnalyzerTypes()
        {
            if (this._analyzerTypes != null)
            {
                return this._analyzerTypes;
            }

            List<Type> ret = this._analyzerTypes = new List<Type>();
            Type baseType = typeof(Analyzer);
            Type[] types = typeof(Analyzer).Assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(baseType))
                {
                    ret.Add(type);
                }
            }

            ret.Sort((a, b) =>
            {
                Analyzer analyzerA = a.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Analyzer;
                Analyzer analyzerB = b.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Analyzer;
                return analyzerA.Priority - analyzerB.Priority;
            });
            return ret;
        }
        #endregion

    }
}
