using System;
using System.Collections.Generic;

namespace TypeScript.Syntax
{
    public static class ClassDeclarationExtension
    {
        public static Node GetMember(this ClassDeclaration classDeclaration, string name)
        {
            if (classDeclaration.Members != null)
            {
                foreach (var member in classDeclaration.Members)
                {
                    if (member is MethodDeclaration)
                    {
                        var methodDeclaration = member as MethodDeclaration;
                        if (methodDeclaration.Name.Text == name)
                        {
                            return methodDeclaration;
                        }
                    }
                    if (member is GetAccessor)
                    {
                        var getAccessor = member as GetAccessor;
                        if (getAccessor.Name.Text == name)
                        {
                            return getAccessor;
                        }
                    }
                    if (member is SetAccessor)
                    {
                        var setAccessor = member as SetAccessor;
                        if (setAccessor.Name.Text == name)
                        {
                            return setAccessor;
                        }
                    }
                    if (member is GetSetAccessor)
                    {
                        var getSetAccessor = member as GetSetAccessor;
                        if (getSetAccessor.Name.Text == name)
                        {
                            return getSetAccessor;
                        }
                    }
                    if (member is PropertyDeclaration)
                    {
                        var propertyDeclaration = member as PropertyDeclaration;
                        if (propertyDeclaration.Name.Text == name)
                        {
                            return propertyDeclaration;
                        }
                    }
                }
            }
            return null;
        }
    }
}
