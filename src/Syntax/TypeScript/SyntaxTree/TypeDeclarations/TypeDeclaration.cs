using System.Collections.Generic;

namespace TypeScript.Syntax
{
    public class TypeDeclaration : Node
    {
        public virtual Node Name
        {
            get;
            protected set;
        }
    }
}
