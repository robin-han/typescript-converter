using System;
using System.Diagnostics;

using TypeScript.Syntax;

namespace GrapeCity.Syntax.Converter.Source.TypeScript.Analysis
{
    public class GetSetAccessorSyntaxNodeInformation
    {
        private GetAccessor getAccessorSyntaxNode;
        private SetAccessor setAccessorSyntaxNode;

        public GetSetAccessorSyntaxNodeInformation(GetAccessor getAccessorSyntaxNode)
        {
            this.getAccessorSyntaxNode = getAccessorSyntaxNode;
        }
        public GetSetAccessorSyntaxNodeInformation(SetAccessor setAccessorSyntaxNode)
        {
            this.setAccessorSyntaxNode = setAccessorSyntaxNode;
        }
        public GetSetAccessorSyntaxNodeInformation(GetAccessor getAccessorSyntaxNode, SetAccessor setAccessorSyntaxNode)
        {
            Debug.Assert(getAccessorSyntaxNode.Ancestor<ClassDeclaration>() == setAccessorSyntaxNode.Ancestor<ClassDeclaration>());
            this.getAccessorSyntaxNode = getAccessorSyntaxNode;
            this.setAccessorSyntaxNode = setAccessorSyntaxNode;
        }

        public GetAccessor GetAccessorSyntaxNode
        {
            get { return this.getAccessorSyntaxNode; }
            set
            {
                if (this.getAccessorSyntaxNode != value)
                {
                    if ((this.getAccessorSyntaxNode != null) && (value != null))
                    {
                        throw new InvalidOperationException();
                    }

                    if ((this.setAccessorSyntaxNode == null) && (value == null))
                    {
                        throw new InvalidOperationException();
                    }

                    if ((value != null) && (this.ClassDeclaration != value.Ancestor<ClassDeclaration>()))
                    {
                        throw new InvalidOperationException();
                    }

                    this.getAccessorSyntaxNode = value;
                }
            }
        }
        public SetAccessor SetAccessorSyntaxNode
        {
            get { return this.setAccessorSyntaxNode; }
            set
            {
                if (this.setAccessorSyntaxNode != value)
                {
                    if ((this.setAccessorSyntaxNode != null) && (value != null))
                    {
                        throw new InvalidOperationException();
                    }

                    if ((this.getAccessorSyntaxNode == null) && (value == null))
                    {
                        throw new InvalidOperationException();
                    }

                    if ((value != null) && (this.ClassDeclaration != value.Ancestor<ClassDeclaration>()))
                    {
                        throw new InvalidOperationException();
                    }

                    this.setAccessorSyntaxNode = value;
                }
            }
        }

        public ClassDeclaration ClassDeclaration
        {
            get
            {
                if (this.getAccessorSyntaxNode != null)
                {
                    return this.getAccessorSyntaxNode.Ancestor<ClassDeclaration>();
                }
                else if (this.setAccessorSyntaxNode != null)
                {
                    return this.setAccessorSyntaxNode.Ancestor<ClassDeclaration>();
                }
                return null;
            }
        }
        public String Name
        {
            get
            {
                if (this.getAccessorSyntaxNode != null)
                {
                    return this.getAccessorSyntaxNode.Name.GetName();
                }
                else if (this.setAccessorSyntaxNode != null)
                {
                    return this.setAccessorSyntaxNode.Name.GetName();
                }
                return null;
            }
        }
    }
}
