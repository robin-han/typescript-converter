using TypeScript.Syntax.Analysis;
using TypeScript.Syntax.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeScript.Syntax
{
    public class Project : IProject
    {
        #region Fields
        private (List<Node> declarations, List<string> names)? _typeDeclarations;
        private List<Node> _globalFunctions;
        private List<Document> _validDocuments;
        #endregion

        #region Constructor
        public Project(string path, List<Document> documents)
        {
            this.Path = path;
            this.Documents = documents;
            this._typeDeclarations = null;
            this._globalFunctions = null;
            this._validDocuments = null;

            foreach (Document doc in documents)
            {
                doc.Project = this;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public string GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the project's path.
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets all documents.
        /// </summary>
        public List<Document> Documents
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the sample documents
        /// </summary>
        public List<Document> SampleDocuments
        {
            get
            {
                if (this._validDocuments != null)
                {
                    return this._validDocuments;
                }
                return this.Documents;
            }
            set
            {
                this._validDocuments = value.FindAll(doc => this.Documents.Contains(doc));
            }
        }

        /// <summary>
        ///  Gets all the type names in the project.
        /// </summary>
        public List<string> TypeDeclarationNames
        {
            get
            {
                return this.GetTypeDeclarations().names;
            }
        }

        /// <summary>
        /// Gets all type nodes in the project.
        /// </summary>
        public List<Node> TypeDeclarations
        {
            get
            {
                return this.GetTypeDeclarations().declarations;
            }
        }

        /// <summary>
        /// Get all global functions in the project.
        /// </summary>
        /// <returns></returns>
        public List<Node> Functions
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

        /// <summary>
        /// Gets or sets the project's converter.
        /// </summary>
        public IConverter Converter
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Normalize docs
        /// </summary>
        /// <param name="docs"></param>
        public void Normalize(List<Document> docs)
        {
            List<Normalizer> analyzers = new List<Normalizer>();
            Type baseType = typeof(Normalizer);
            Type[] types = typeof(Normalizer).Assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(baseType))
                {
                    analyzers.Add((Normalizer)type.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes));
                }
            }
            analyzers.Sort((a, b) =>
            {
                return a.Priority - b.Priority;
            });

            foreach (var analyzer in analyzers)
            {
                foreach (Document doc in docs)
                {
                    analyzer.Analyze(doc.Source);
                }
            }
        }

        /// <summary>
        /// Gets the document by its path.
        /// </summary>
        /// <param name="path">The import/export path.</param>
        /// <returns>The document.</returns>
        public Document GetDocument(string path)
        {
            string docPath = path.EndsWith(".ts.json") ? path : path + ".ts.json";
            return this.Documents.Find(doc => IsSamePath(doc.Path, docPath));
        }

        /// <summary>
        /// Gets the document which the class, interface, enum or typeAlias within.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The document where the type in</returns>
        public Document GetTypeDeclarationDocument(string typeName)
        {
            Node typeDeclaration = GetTypeDeclaration(typeName);
            return typeDeclaration?.Document;
        }

        /// <summary>
        /// Gets the node by type name.
        /// </summary>
        /// <param name="typeName">The type name</param>
        /// <returns>The type node</returns>
        public Node GetTypeDeclaration(string typeName)
        {
            int index = this.TypeDeclarationNames.IndexOf(typeName);
            if (index >= 0)
            {
                return this.TypeDeclarations[index];
            }
            return null;
        }

        /// <summary>
        /// Gets the function declaration by name
        /// </summary>
        /// <param name="fnName">The function declaration name.</param>
        /// <returns>The function declaration</returns>
        public Node GetFunctionDeclaration(string fnName)
        {
            foreach (FunctionDeclaration fn in this.Functions)
            {
                if (fn.Name.Text == fnName)
                {
                    return fn;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the enum declaration node with the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EnumDeclaration GetEnum(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (Node node in this.TypeDeclarations)
            {
                if (node.Kind == NodeKind.EnumDeclaration)
                {
                    EnumDeclaration @enum = (EnumDeclaration)node;
                    if (@enum.NameText == name)
                    {
                        return @enum;
                    }
                }
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

            foreach (Node node in this.TypeDeclarations)
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

            foreach (Node node in this.TypeDeclarations)
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

            foreach (HeritageClause inherit in classNode.HeritageClauses)
            {
                if (inherit.Token == NodeKind.ExtendsKeyword && inherit.Types.Count > 0)
                {
                    string typeName = TypeHelper.GetTypeDeclarationName(inherit.Types[0]);
                    return this.GetClass(typeName);
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
                        string className = TypeHelper.GetTypeDeclarationName(type);
                        Node @base = this.GetTypeDeclaration(className);
                        if (@base != null)
                        {
                            ret.Add(@base);
                            List<Node> heritageClauses =
                                  @base.Kind == NodeKind.ClassDeclaration
                                ? ((ClassDeclaration)@base).HeritageClauses
                                : ((InterfaceDeclaration)@base).HeritageClauses;

                            foreach (HeritageClause nextInherit in heritageClauses)
                            {
                                heritages.Enqueue(nextInherit);
                            }
                        }
                    }
                    else
                    {
                        string interfaceName = TypeHelper.GetTypeDeclarationName(type);
                        InterfaceDeclaration baseInterface = this.GetInterface(interfaceName);
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
        public List<string> GetReferences(List<string> typeNames)
        {
            List<string> ret = new List<string>();
            foreach (string name in typeNames)
            {
                ret.Add(name);
                this.FillReferences(name, ret);
            }
            return ret;
        }

        /// <summary>
        /// Gets a type's reference
        /// </summary>
        /// <param name="name">The type name.</param>
        /// <param name="results">The referenced types.</param>
        private void FillReferences(string typeName, List<string> result)
        {
            var (declarations, names) = this.GetTypeDeclarations();
            int index = names.IndexOf(typeName);
            if (index == -1)
            {
                return;
            }

            Node rootNode = declarations[index];
            Document document = rootNode.Document;
            if (document == null || !this.SampleDocuments.Contains(document))
            {
                return;
            }

            foreach (string name in document.GetReferenceTypes())
            {
                if (!result.Contains(name) && names.Contains(name))
                {
                    result.Add(name);
                    this.FillReferences(name, result);
                }
            }
        }

        /// <summary>
        /// Get all type nodes(class, interfact, enum etc.) in the project.
        /// </summary>
        /// <returns></returns>
        private (List<Node> declarations, List<string> names) GetTypeDeclarations()
        {
            if (this._typeDeclarations.HasValue)
            {
                return (this._typeDeclarations.Value.declarations, this._typeDeclarations.Value.names);
            }

            //
            List<Node> declarations = new List<Node>();
            List<string> names = new List<string>();
            foreach (Document doc in this.Documents)
            {
                foreach (Node typeDeclaration in doc.TypeDeclarations)
                {
                    string name = typeDeclaration.GetName();
                    declarations.Add(typeDeclaration);
                    names.Add(name);
                }
            }

            this._typeDeclarations = (declarations, names);
            return (declarations, names);
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
                nodes.AddRange(doc.Functions);
            }
            return nodes;
        }

        /// <summary>
        /// Indicates is the same path.
        /// </summary>
        private bool IsSamePath(string path1, string path2)
        {
            return System.IO.Path.GetFullPath(path1) == System.IO.Path.GetFullPath(path2);
        }

        #endregion
    }
}
