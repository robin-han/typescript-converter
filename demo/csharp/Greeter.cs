using System.Linq;
using TypeScript.CSharp;

namespace Demo
{
    public class Greeter : Object, IGreeter
    {
        private String _useName;
        private String _greeting;
        public Greeter()
        {
            this._useName = "";
            this._greeting = "";
        }

        public String userName()
        {
            return this._useName;
        }

        public void userName(String value)
        {
            this._useName = value;
        }

        public String greeting
        {
            get
            {
                return this._greeting;
            }

            set
            {
                this._greeting = value;
            }
        }

        public String greet()
        {
            return "Hello, " + this.greeting;
        }
    }
}