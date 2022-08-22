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
        private string _path;
        private List<Document> _documents;
        private List<Document> _includeDocuments;

        private (List<Node> nodes, List<string> names)? _typeNodes;
        private List<Type> _analyzerTypes;
        private List<Node> _globalFunctions;
        #endregion

        #region Constructor
        public Project(string path, List<Document> documents, List<Document> includedDocuments)
        {
            this._path = path;
            this._documents = documents;
            this._includeDocuments = includedDocuments;
            this._typeNodes = null;
            this._analyzerTypes = null;
            this._globalFunctions = null;

            foreach (Document doc in documents)
            {
                doc.Project = this;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the project's path.
        /// </summary>
        public string Path
        {
            get
            {
                return this._path;
            }
        }

        /// <summary>
        /// Gets all documents.
        /// </summary>
        public List<Document> Documents
        {
            get
            {
                return this._documents;
            }
        }

        /// <summary>
        /// Gets transform documents.
        /// </summary>
        public List<Document> IncludeDocuments
        {
            get
            {
                return this._includeDocuments;
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
        /// Gets all type nodes in the project.
        /// </summary>
        public List<Node> TypeNodes
        {
            get
            {
                return this.GetAllTypes().nodes;
            }
        }

        /// <summary>
        /// Get all global functions in the project.
        /// </summary>
        /// <returns></returns>
        public List<Node> GlobalFunctions
        {
            get
            {
                if (this._globalFunctions == null)
                {
                    this._globalFunctions = this.GetGlobalFunctions();
                }
                return this._globalFunctions;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add document to transform collection.
        /// </summary>
        /// <param name="doc">The document.</param>
        public void AddIncludeDocument(Document doc)
        {
            if (!this.IncludeDocuments.Contains(doc))
            {
                this.IncludeDocuments.Add(doc);
            }
        }

        /// <summary>
        /// Gets the document by its path.
        /// </summary>
        /// <param name="path">The import/export path.</param>
        /// <returns>The document.</returns>
        public Document GetDocumentByPath(string path)
        {
            string docPath = (path.EndsWith(".ts.json") ? path : path + ".ts.json");
            return this.Documents.Find(doc => doc.Path == docPath);
        }

        /// <summary>
        /// Gets the document which the class, interface, enum or typeAlias within.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The document where the type in</returns>
        public Document GetDocumentByType(string typeName)
        {
            int index = this.TypeNames.IndexOf(typeName);
            if (index >= 0)
            {
                return this.TypeNodes[index].Document;
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

            foreach (Node node in this.TypeNodes)
            {
                if (node.Kind == NodeKind.ClassDeclaration)
                {
                    ClassDeclaration cls = (ClassDeclaration)node;
                    if (cls.NameText == name)
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

            foreach (Node node in this.TypeNodes)
            {
                if (node.Kind == NodeKind.InterfaceDeclaration)
                {
                    InterfaceDeclaration itfs = (InterfaceDeclaration)node;
                    if (itfs.NameText == name)
                    {
                        return itfs;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets class's base class.
        /// </summary>
        /// <param name="classNode">The class node to get its base.</param>
        /// <returns>The base class.</returns>
        public ClassDeclaration GetBaseClass(ClassDeclaration classNode)
        {
            if (classNode == null)
            {
                return null;
            }

            Document document = classNode.Document;
            foreach (HeritageClause inherit in classNode.HeritageClauses)
            {
                if (inherit.Token == NodeKind.ExtendsKeyword && inherit.Types.Count > 0)
                {
                    string typeName = TypeHelper.ToShortName(inherit.Types[0].Text);
                    return this.GetClass(document.GetTypeDefinitionName(typeName));
                }
            }
            return null;
        }

        /// <summary>
        /// Ges all its inherited classes.
        /// </summary>
        /// <param name="classNode">The class node</param>
        /// <returns>Inherited classes</returns>
        public List<ClassDeclaration> GetInheritClasses(ClassDeclaration classNode)
        {
            List<ClassDeclaration> ret = new List<ClassDeclaration>();
            foreach (Node baseNode in this.GetInherits(classNode))
            {
                if (baseNode.Kind == NodeKind.ClassDeclaration)
                {
                    ret.Add((ClassDeclaration)baseNode);
                }
            }
            return ret;
        }

        /// <summary>
        /// Ges all its inherited interfaces.
        /// </summary>
        /// <param name="classNode">The class or interface node</param>
        /// <returns>Inherited interfaces.</returns>
        public List<InterfaceDeclaration> GetInheritInterfaces(Node node)
        {
            List<InterfaceDeclaration> ret = new List<InterfaceDeclaration>();
            foreach (Node baseNode in this.GetInherits(node))
            {
                if (baseNode.Kind == NodeKind.InterfaceDeclaration)
                {
                    ret.Add((InterfaceDeclaration)baseNode);
                }
            }
            return ret;
        }


        /// <summary>
        /// Ges all its inherited interfaces and classes.
        /// </summary>
        /// <param name="classNode">The class or interface node</param>
        /// <returns>Inherited interfaces and classes.</returns>
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
                        string className = TypeHelper.ToShortName(type.Text);
                        ClassDeclaration baseClass = this.GetClass(type.Document.GetTypeDefinitionName(className));
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
                        string interfaceName = TypeHelper.ToShortName(type.Text);
                        InterfaceDeclaration baseInterface = this.GetInterface(type.Document.GetTypeDefinitionName(interfaceName));
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
        /// Gets a type's reference
        /// </summary>
        /// <param name="name">The type name.</param>
        /// <param name="results">The referenced types.</param>
        private void GetReferences(string typeName, List<string> result)
        {
            var (nodes, names) = this.GetAllTypes();
            int index = names.IndexOf(typeName);
            if (index == -1)
            {
                return;
            }

            Node rootNode = nodes[index];
            Document document = rootNode.Document;
            if (document == null || !this.IncludeDocuments.Contains(document))
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
                
                string name = TypeHelper.ToShortName(type.Text);
                name = document.GetTypeDefinitionName(name);
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
            foreach (Document doc in this.Documents)
            {
                List<Node> docTypes = doc.TypeNodes;
                foreach (Node node in docTypes)
                {
                    string name = TypeHelper.GetTypeName(node);
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

        /// <summary>
        /// Gets all global functions in the project.
        /// </summary>
        /// <returns></returns>
        private List<Node> GetGlobalFunctions()
        {
            List<Node> nodes = new List<Node>();
            foreach (Document doc in this.Documents)
            {
                nodes.AddRange(doc.GlobalFunctions);
            }
            return nodes;
        }

        /// <summary>
        /// Gets all analyzer types of the project
        /// </summary>
        /// <returns></returns>
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
