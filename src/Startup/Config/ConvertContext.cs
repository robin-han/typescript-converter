using TypeScript.Syntax;
using TypeScript.Syntax.Converter;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter
{
    class ConvertContext : IConvertContext
    {
        #region Fields
        private IProject _project;
        private ExecuteArgument _arg;

        private string _ns;
        private bool _preferTsType;
        private bool _preferTsAdvancedType;
        private List<string> _usings;
        private Dictionary<string, string> _nsMappings;
        private List<string> _qualifiedNames;
        private List<string> _excludeTypes;
        #endregion

        public ConvertContext(IProject project, ExecuteArgument arg)
        {
            this._project = project;
            this._arg = arg;

            this._ns = string.Empty;
            this._preferTsType = true;
            this._preferTsAdvancedType = false;
            this._usings = new List<string>();
            this._nsMappings = new Dictionary<string, string>();
            this._qualifiedNames = new List<string>();
            this._excludeTypes = new List<string>();
        }

        #region Properties
        /// <summary>
        /// Get the project.
        /// </summary>
        public IProject Project
        {
            get { return this._project; }
        }

        public ExecuteArgument ExecArgument
        {
            get { return this._arg; }
        }
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string Namespace
        {
            get
            {
                return this._ns;
            }
            set
            {
                this._ns = value;
            }
        }

        /// <summary>
        /// Gets or sets whehter convert to typescript type. 
        /// <b>true</b> will convert to typescript primitive type, otherwise will convert to csharp primitive type.
        /// </summary>
        public bool TypeScriptType
        {
            get
            {
                return _preferTsType;
            }
            set
            {
                this._preferTsType = value;
            }
        }

        /// <summary>
        /// Gets or sets whehter convert to advanced typescript types. 
        /// <b>true</b> will convert to a dynamic if set to false otherwise convert to a generic type adapter type.
        /// </summary>
        public bool TypeScriptAdvancedType
        {
            get
            {
                return _preferTsAdvancedType;
            }
            set
            {
                this._preferTsAdvancedType = value;
            }
        }

        /// <summary>
        /// Gets or sets the usings for every file.
        /// </summary>
        public List<string> Usings
        {
            get
            {
                return this._usings;
            }
            set
            {
                this._usings = value ?? throw new ArgumentNullException("usings");
            }
        }

        /// <summary>
        /// Gets or sets the omitted qualified names.
        /// </summary>
        public List<string> QualifiedNames
        {
            get
            {
                return this._qualifiedNames;
            }
            set
            {
                this._qualifiedNames = value ?? throw new ArgumentNullException("omiited qualified names");
            }
        }

        /// <summary>
        /// Gets or sets the exclude types.
        /// </summary>
        public List<string> ExcludeTypes
        {
            get
            {
                return this._excludeTypes;
            }
            set
            {
                this._excludeTypes = value ?? throw new ArgumentNullException("exclude types");
            }
        }

        /// <summary>
        /// Gets the document's output.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public IOutput GetOutput(Document doc)
        {
            return ExecArgument.GetOutput(doc);
        }

        /// <summary>
        /// Trim type name by qualified names.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public string TrimTypeName(string typeName)
        {
            string[] parts = typeName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var omitedName in this.QualifiedNames)
            {
                string prefixText = omitedName + '.';
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].StartsWith(prefixText))
                    {
                        parts[i] = parts[i].Substring(prefixText.Length);
                    }
                }
            }

            return string.Join(' ', parts);
        }
        #endregion
    }


}
