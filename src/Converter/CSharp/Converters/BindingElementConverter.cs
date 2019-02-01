using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp
{
    public class BindingElementConverter : Converter
    {
        public CSharpSyntaxNode Convert(BindingElement node)
        {
            //TODO: NOT SUPPORT  let [deltaX, deltaY] = this._getWheelDelte(evt);
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }
    }
}

