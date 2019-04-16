using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class ConverterContext
    {
        #region Fields
        private readonly Config _config;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public ConverterContext() : this(new Config())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public ConverterContext(Config config)
        {
            this._config = config ?? throw new ArgumentNullException("config");
        }
        #endregion

        #region Properties
        public Config Config
        {
            get { return this._config; }
        }
        #endregion

        #region Methods

        #endregion
    }
}
