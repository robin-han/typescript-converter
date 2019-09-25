﻿using TypeScript.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Converter.CSharp
{
    public class ImportDeclarationConverter : Converter
    {
        public SyntaxList<UsingDirectiveSyntax> Convert(ImportDeclaration node)
        {
            return node.ImportClause.ToCsNode<SyntaxList<UsingDirectiveSyntax>>();
        }
    }
}
