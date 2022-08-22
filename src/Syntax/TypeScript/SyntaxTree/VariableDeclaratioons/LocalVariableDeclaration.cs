namespace TypeScript.Syntax
{
    public class LocalVariableDeclaration : Node
    {
        public virtual Node Name
        {
            get;
            protected set;
        }

        public virtual Node Type
        {
            get;
            protected set;
        }

        public virtual Node Initializer
        {
            get;
            protected set;
        }
    }
}
