using System;

namespace TypeScript.Syntax
{
    public class NodeKindAttribute : Attribute
    {
        public NodeKind Kind { get; private set; }

        public NodeKindAttribute(NodeKind kind)
        {
            this.Kind = kind;
        }
    }
}

