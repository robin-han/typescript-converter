using System.Linq;
using TypeScriptObject;

namespace Demo
{
    public interface IGreeter
    {
        String greeting
        {
            get;
            set;
        }

        String greet();
    }
}