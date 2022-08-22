namespace TypeScript.Syntax
{
    public class UnexpectedMemberSyntaxValidator
    {
        public UnexpectedMemberSyntaxValidator()
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
            else if (typeDeclaration is InterfaceDeclaration)
            {
                var interfaceDeclaration = typeDeclaration as InterfaceDeclaration;
                var members = interfaceDeclaration.Members;
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
            if (member is GetAccessor)
            {
                var getAccessor = member as GetAccessor;
                if (getAccessor.Type == null)
                {
                    context.Errors.Add(new UnexpectedMemberSyntaxValidatorError(getAccessor.Root, getAccessor.Pos));
                }
            }
            else if (member is SetAccessor)
            {
                var setAccessor = member as SetAccessor;
                if (setAccessor.Type == null)
                {
                    context.Errors.Add(new UnexpectedMemberSyntaxValidatorError(setAccessor.Root, setAccessor.Pos));
                }
            }
            else if (member is MethodDeclaration)
            {
                var methodDeclaration = member as MethodDeclaration;
                if (methodDeclaration.Type == null)
                {
                    context.Errors.Add(new UnexpectedMemberSyntaxValidatorError(methodDeclaration.Root, methodDeclaration.Pos));
                }
            }
            else if (member is PropertyDeclaration)
            {
                var propertyDeclaration = member as PropertyDeclaration;
                if ((propertyDeclaration.Type == null) && (propertyDeclaration.Initializer == null))
                {
                    context.Errors.Add(new UnexpectedMemberSyntaxValidatorError(propertyDeclaration.Root, propertyDeclaration.Pos));
                }
            }
        }
    }
}
