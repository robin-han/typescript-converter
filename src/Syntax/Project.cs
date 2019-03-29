using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GrapeCity.CodeAnalysis.TypeScript.Syntax
{
    public class Project
    {
        #region Fields
        private readonly List<Document> _documents;
        private (List<Node> nodes, List<string> names)? _typeNodes;
        #endregion

        #region Constructor
        public Project(List<Document> documents)
        {
            this._documents = documents;
            this._typeNodes = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
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
        #endregion

        #region Methods
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

            List<Node> types = new List<Node>();
            nodes[index].Descendants(n =>
            {
                if (n.GetValue("Type") is Node t)
                {
                    types.Add(t);
                }
                else if (n.GetValue("Types") is List<Node> ts)
                {
                    types.AddRange(ts);
                }
                return false;
            });
            foreach (Node type in types)
            {
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
            foreach (Document doc in this.Documents)
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
        #endregion
    }
}
