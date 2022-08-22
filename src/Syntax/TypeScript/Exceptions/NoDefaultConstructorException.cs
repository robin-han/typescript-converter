using System;

namespace TypeScript.Syntax
{
    public class NoDefaultConstructorException : Exception
    {
        public NoDefaultConstructorException()
            : base()
        { }
    }
}
