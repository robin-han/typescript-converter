using System;

namespace TypeScript.Syntax
{
    public class UnexpectedMemberSyntaxValidatorError : ValidatorError
    {
        public UnexpectedMemberSyntaxValidatorError(SourceFile sourceFile, int location)
            : base(sourceFile, location)
        {
        }

        public override string Message
        {
            get
            {
                return String.Format("An unexpected member syntax in position ({0}:{1}) of the file '{2}' is found.", this.LineIndex + 1, this.ColumnIndex + 1, this.SourceFile.FileName);
            }
        }
    }
}
