using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class ConverterContext
    {
        #region Fields
        private readonly Project _project;
        private readonly ConverterConfig _config;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public ConverterContext(Project project) : this(project, new ConverterConfig())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="config"></param>
        public ConverterContext(Project project, ConverterConfig config)
        {
            this._project = project;
            this._config = config;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get the config
        /// </summary>
        public ConverterConfig Config
        {
            get { return this._config; }
        }

        /// <summary>
        /// Get the project.
        /// </summary>
        public Project Project
        {
            get { return this._project; }
        }
        #endregion
    }
}
