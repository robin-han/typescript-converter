namespace TypeScript.Syntax
{
    public class UnexpectedSemicolonValidator
    {
        public UnexpectedSemicolonValidator()
        {
        }

        public ValidatorError[] Validate(TypeDeclaration[] typeDeclarations)
        {
            var context = new ValidatorContext();

            foreach (var typeDeclaration in typeDeclarations)
            {
                if (typeDeclaration != null)
                {
                    this.ValidateTypeDeclaration(typeDeclaration, context);
                }
            }

            return context.Errors.ToArray();
        }

        protected void ValidateTypeDeclaration(TypeDeclaration typeDeclaration, ValidatorContext context)
        {
            if (typeDeclaration is ClassDeclaration)
            {
                var classDeclaration = typeDeclaration as ClassDeclaration;
                var members = classDeclaration.Members;
                if (members != null)
                {
                    foreach (var member in members)
                    {
                        this.ValidateMember(member, context);
                    }
                }
            }
        }
        protected void ValidateMember(Node member, ValidatorContext context)
        {
            if (member is SemicolonClassElement)
            {
                context.Errors.Add(new UnexpectedSemicolonValidatorError(member.Root, member.Pos));
            }
        }
    }
}
