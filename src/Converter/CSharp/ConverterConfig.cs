using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class ConverterConfig
    {
        #region Fields
        private string _ns;
        private bool _preferTsType;
        private List<string> _usings;
        private Dictionary<string, string> _nsMappings;
        private List<string> _omittedQualifiedNames;
        private List<string> _excludeTypes;
        #endregion

        public ConverterConfig()
        {
            this._ns = string.Empty;
            this._preferTsType = true;
            this._usings = new List<string>();
            this._nsMappings = new Dictionary<string, string>();
            this._omittedQualifiedNames = new List<string>();
            this._excludeTypes = new List<string>();
        }

        #region Properties
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
        public bool PreferTypeScriptType
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
        /// Gets or sets the namespace mappings.
        /// </summary>
        public Dictionary<string, string> NamespaceMappings
        {
            get
            {
                return this._nsMappings;
            }
            set
            {
                this._nsMappings = value ?? throw new ArgumentNullException("namespace mappings");
            }
        }

        /// <summary>
        /// Gets or sets the omitted qualified names.
        /// </summary>
        public List<string> OmittedQualifiedNames
        {
            get
            {
                return this._omittedQualifiedNames;
            }
            set
            {
                this._omittedQualifiedNames = value ?? throw new ArgumentNullException("omiited qualified names");
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
        #endregion
    }
}
