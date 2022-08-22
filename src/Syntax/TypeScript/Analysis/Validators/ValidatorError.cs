namespace TypeScript.Syntax
{
    public class ValidatorError
    {
        private bool rowAndColumnIndexDirty = true;
        private int lineIndex = 0;
        private int columnIndex = 0;

        public SourceFile SourceFile { get; private set; }
        public int Location { get; private set; }
        public virtual string Message { get; private set; }

        public ValidatorError(SourceFile sourceFile, int location, string message = "")
        {
            this.SourceFile = sourceFile;
            this.Location = location;
            this.Message = message;
        }

        public int LineIndex
        {
            get
            {
                if (this.rowAndColumnIndexDirty)
                {
                    this.CalculateRowAndColumnIndex();
                    this.rowAndColumnIndexDirty = false;
                }
                return this.lineIndex;
            }
        }
        public int ColumnIndex
        {
            get
            {
                if (this.rowAndColumnIndexDirty)
                {
                    this.CalculateRowAndColumnIndex();
                    this.rowAndColumnIndexDirty = false;
                }
                return this.columnIndex;
            }
        }

        protected void CalculateRowAndColumnIndex()
        {
            var lines = SourceFile.Text.Split('\n');
            var characterCount = 0;
            for (var rowIndex = 0; rowIndex < lines.Length; rowIndex++)
            {
                this.columnIndex = this.Location - characterCount;
                characterCount += lines[rowIndex].Length + System.Environment.NewLine.Length;
                if (this.Location < characterCount)
                {
                    this.lineIndex = rowIndex;
                    break;
                }
            }
        }
    }
}
