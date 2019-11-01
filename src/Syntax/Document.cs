using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;


namespace TypeScript.Syntax
{
    public class Document
    {
        #region Fields
        private Node _root;
        private string _path;
        private Project _project;
        #endregion

        #region Constructor
        public Document(string path, Node root)
        {
            this._path = path;
            this._root = root;
            this._project = null;

            root.Document = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the document's full path.
        /// </summary>
        public string Path
        {
            get
            {
                return this._path;
            }
        }

        /// <summary>
        /// Gets the document's relative path.
        /// </summary>
        public string RelativePath
        {
            get
            {
                if (this.Project != null && !string.IsNullOrEmpty(this.Project.Path))
                {
                    string docDirectory = System.IO.Path.GetDirectoryName(this.Path);
                    string relativePath = System.IO.Path.GetRelativePath(this.Project.Path, docDirectory);
                    return (relativePath == "." ? string.Empty : relativePath);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the document's root node.
        /// </summary>
        public Node Root
        {
            get
            {
                return this._root;
            }
        }

        /// <summary>
        /// Gets the document's project.
        /// </summary>
        public Project Project
        {
            get
            {
                return this._project;
            }
            set
            {
                this._project = value;
            }
        }

        /// <summary>
        /// Gets all type nodes(class, interfact, enum, type alias, etc.) in the document.
        /// </summary>
        public List<Node> TypeNodes
        {
            get
            {
                if ((this.Root is SourceFile sourceFile))
                {
                    return sourceFile.TypeNodes;
                }
                return new List<Node>();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the definition name of typename.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The definition name.</returns>
        public string GetTypeDefinitionName(string typeName)
        {
            Node definition = this.GetTypeDefinition(typeName);
            if (definition != null)
            {
                return (string)definition.GetValue("NameText");
            }
            return typeName;
        }

        /// <summary>
        /// Gets type definition.
        /// </summary>
        /// <param name="typeName">The type name</param>
        /// <returns>The type definition</returns>
        public Node GetTypeDefinition(string typeName)
        {
            if (!(this.Root is SourceFile sourceFile))
            {
                return null;
            }

            Node definition = sourceFile.GetOwnModuleTypeDefinition(typeName);
            if (definition != null)
            {
                return definition;
            }

            definition = sourceFile.GetImportModuleTypeDefinition(typeName);
            if (definition != null)
            {
                return definition;
            }

            int index = this.Project == null ? -1 : this.Project.TypeNames.IndexOf(typeName);
            if (index >= 0)
            {
                return this.Project.TypeNodes[index];
            }

            return null;
        }

        /// <summary>
        /// Gets export type definition.
        /// </summary>
        /// <param name="typeName">The export type name.</param>
        /// <returns>The definition.</returns>
        public Node GetExportTypeDefinition(string typeName)
        {
            if (!(this.Root is SourceFile sourceFile))
            {
                return null;
            }

            return sourceFile.GetExportModuleTypeDefinition(typeName);
        }

        /// <summary>
        /// Gets the export default type definition.
        /// </summary>
        /// <returns></returns>
        public Node GetExportDefaultTypeDefinition()
        {
            if (!(this.Root is SourceFile sourceFile))
            {
                return null;
            }

            return sourceFile.GetExportDefaultModuleTypeDefinition();
        }
        #endregion
    }
}
