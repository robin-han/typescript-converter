using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TypeScript.Syntax;
using TypeScript.Syntax.Converter;
using com.sun.tools.javac.tree;
using com.sun.source.tree;
using com.sun.tools.javac.util;
using static com.sun.tools.javac.tree.JCTree;


namespace TypeScript.Converter.Java
{
    public class ModuleBlockConverter : NodeConverter
    {
        private static Dictionary<Document, IOutput> cache = new Dictionary<Document, IOutput>();

        public List<JCTree> Convert(ModuleBlock node)
        {
            List<JCTree> defs = new List<JCTree>();
            // package
            string pkg = this.Context.Namespace;
            if (string.IsNullOrEmpty(pkg))
            {
                pkg = node.Document.GetPackageName();
            }
            defs.Add(TreeMaker.PackageDecl(Nil<JCAnnotation>(), TreeMaker.Ident(Names.fromString(pkg))));

            // alias
            defs.AddRange(node.TypeAliases.ToJavaSyntaxTrees<JCTree>());

            // imports
            List<string> usings = this.Context.Usings;
            List<string> imports = new List<string>(usings);
            Project project = node.Document.Project;
            foreach (var refType in node.Document.GetReferenceTypes())
            {
                Document importDoc = project.GetTypeDeclarationDocument(refType);
                string import = importDoc.GetPackageName();
                if (import != pkg && !imports.Contains($"{import}.*"))
                {
                    IOutput output;
                    if (cache.ContainsKey(importDoc))
                    {
                        output = cache[importDoc];
                    }
                    else
                    {
                        output = this.Context.GetOutput(importDoc);
                        cache[importDoc] = output;
                    }

                    if (!output.Flat)
                    {
                        imports.Add($"{import}.*");
                    }
                }
            }
            imports.Sort(usings.Count, imports.Count - usings.Count, new ImportsComparer());

            string staticPrefix = "static ";
            foreach (var import in imports)
            {
                if (import.StartsWith(staticPrefix))
                {
                    defs.Add(TreeMaker.Import(TreeMaker.Ident(Names.fromString(import.Substring(staticPrefix.Length))), true));
                }
                else
                {
                    defs.Add(TreeMaker.Import(TreeMaker.Ident(Names.fromString(import)), false));
                }
            }

            // types
            List<Node> types = this.FilterTypes(node.TypeDeclarations);
            defs.AddRange(types.ToJavaSyntaxTrees<JCTree>());

            // TODO: remove empty folder's import??? 
            // Refactor JavaConverter to adds timeline after java ast is builded

            return defs;
        }


        private class ImportsComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, new System.Globalization.CultureInfo("en-US"), System.Globalization.CompareOptions.Ordinal);
            }
        }
    }
}

