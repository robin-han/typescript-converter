using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter
{
    class Output
    {
        public Output()
        {
            this.FlatOutput = false;
            this.Namespace = "";
            this.PreferTypeScriptType = true;
            this.Path = "";
            this.Patterns = new List<string>();
            this.Usings = new List<string>();
        }

        /// <summary>
        /// Gets or sets whether flat output
        /// </summary>
        public bool FlatOutput
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
        public bool PreferTypeScriptType
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
    }
}
