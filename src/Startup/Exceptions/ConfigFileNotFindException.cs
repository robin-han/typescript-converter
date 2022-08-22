using System;

namespace GrapeCity.Syntax.Converter.Console.Exceptions
{
    internal class ConfigFileNotFindException : Exception
    {
        public ConfigFileNotFindException(string filePath)
            : base(String.Format("The config file \"{0}\" is not found.", filePath))
        {
        }
    }
}