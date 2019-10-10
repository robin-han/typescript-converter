using System.Linq;
using TypeScript.CSharp;

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