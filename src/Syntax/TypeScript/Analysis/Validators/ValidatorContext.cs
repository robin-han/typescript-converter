using System.Collections.Generic;

namespace TypeScript.Syntax
{
    public class ValidatorContext
    {
        public List<ValidatorError> Errors { get; private set; }

        public ValidatorContext()
        {
            this.Errors = new List<ValidatorError>();
        }
    }
}
