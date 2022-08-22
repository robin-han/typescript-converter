using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using GrapeCity.Syntax.Converter.Console.Exceptions;

namespace TypeScript.Converter
{
    class ConfigOption
    {
        public SourceOption Source { get; private set; }
        public TargetOption Target { get; private set; }

        public ConfigOption()
        {
            this.Source = new SourceOption();
            this.Target = new TargetOption();

            this.GroupId = null;
            this.OuputLang = Lang.CSharp;
            this.AstFolderPaths = new List<string>();
            this.Exclude = new List<string>();
            this.Samples = new List<string>();
            this.Outputs = new List<Output>();
            this.QualifiedNames = new List<string>();
        }

        #region Properties
        public string GroupId
        {
            get;
             set;
        }

        public Lang OuputLang
        {
            get;
             set;
        }

        public List<string> AstFolderPaths { get;  set; }


        public List<string> Exclude
        {
            get;
             set;
        }

        public List<string> Samples
        {
            get;
             set;
        }

        public List<Output> Outputs
        {
            get;
             set;
        }

        public List<string> QualifiedNames
        {
            get;
             set;
        }
        #endregion

        #region Methods

        public static Lang ParseLang(string value)
        {
            return (Lang)Enum.Parse(typeof(Lang), value, true);
        }
        #endregion
    }
}
