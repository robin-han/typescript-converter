using TypeScript.Syntax;
using TypeScript.Syntax.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace TypeScript.Converter
{
    class Output : IOutput
    {
        #region Constructor
        public Output()
        {
            this.Flat = false;
            this.Namespace = null;
            this.TypeScriptType = true;
            this.TypeScriptAdvancedType = false;
            this.Path = null;
            this.Patterns = new List<string>();
            this.Usings = new List<string>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether flat output
        /// </summary>
        public bool Flat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets output namespace
        /// </summary>
        public string Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether output typescript type or not.
        /// </summary>
        public bool TypeScriptType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whehter output advanced typescript types or not.
        /// </summary>
        public bool TypeScriptAdvancedType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or set output path.
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets output pattern.
        /// </summary>
        public List<string> Patterns
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets output usings
        /// </summary>
        public List<string> Usings
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public bool IsMatch(Document doc)
        {
            foreach (string pattern in this.Patterns)
            {
                //enum keyword
                bool isEnumFile = doc.IsEnumFile;
                bool isEnumKeyword = pattern == "enum";
                bool isEnumPattern = pattern.StartsWith("enum[") && pattern.EndsWith("]");
                bool isEnumMatch = isEnumKeyword || isEnumPattern;
                if (isEnumMatch)
                {
                    if (isEnumFile && isEnumKeyword)
                    {
                        return true;
                    }

                    if (isEnumFile && isEnumPattern)
                    {
                        string enumName = "'" + doc.TypeDeclarationNames[0] + "'";
                        string enumPattern = pattern.Substring(5, pattern.Length - 6);
                        if (enumPattern[0] == '!' && !enumPattern.Substring(1).Split(',', StringSplitOptions.RemoveEmptyEntries).Contains(enumName))
                        {
                            return true;
                        }
                        if (enumPattern.Split(',', StringSplitOptions.RemoveEmptyEntries).Contains(enumName))
                        {
                            return true;
                        }
                    }
                    continue;
                }

                //
                if (FileUtil.IsMatch(doc.Path, pattern))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
