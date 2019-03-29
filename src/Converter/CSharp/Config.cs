using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class Config
    {
        #region Fields
        private string _ns;
        private bool _preferTsType;
        private List<string> _usings;
        private Dictionary<string, string> _nsMappings;
        private List<string> _omittedQualifiedNames;
        private List<string> _excludeTypes;
        #endregion

        public Config()
        {
            this._ns = "";
            this._preferTsType = true;
            this._usings = new List<string>();
            this._nsMappings = new Dictionary<string, string>();
            this._omittedQualifiedNames = new List<string>();
            this._excludeTypes = new List<string>();
        }

        #region Properties
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

        public List<string> Usings
        {
            get
            {
                return this._usings;
            }
            set
            {
                if (value == null)
                {
                    value = new List<string>();
                }
                this._usings = value;
            }
        }

        public Dictionary<string, string> NamespaceMappings
        {
            get
            {
                return this._nsMappings;
            }
            set
            {
                if (value == null)
                {
                    value = new Dictionary<string, string>();
                }
                this._nsMappings = value;
            }
        }

        public List<string> OmittedQualifiedNames
        {
            get
            {
                return this._omittedQualifiedNames;
            }
            set
            {
                if (value == null)
                {
                    value = new List<string>();
                }
                this._omittedQualifiedNames = value;
            }
        }

        public List<string> ExcludeTypes
        {
            get
            {
                return this._excludeTypes;
            }
            set
            {
                if (value == null)
                {
                    value = new List<string>();
                }
                this._excludeTypes = value;
            }
        }
        #endregion
    }
}
