using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Converter
{
    public interface IOutput
    {
        /// <summary>
        /// Indicates whether flat converted source file to one folder.
        /// </summary>
        bool Flat { get; }

        //TODO: Adds Others
    }
}
