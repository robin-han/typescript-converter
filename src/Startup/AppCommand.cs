﻿using GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter
{
    class AppCommand
    {
        #region Fields
        private readonly CommandLineApplication _appCmd;
        #endregion

        #region Constructor
        public AppCommand()
        {
            this._appCmd = this.CreateCommand();
            this._appCmd.OnExecute(new Func<int>(this.OnCommandLineExecute));
        }
        #endregion

        #region Public Methods
        public int Execute(params string[] args)
        {
            try
            {
                this._appCmd.Execute(args);
            }
            catch (Exception ex)
            {
                this.Log(ex.Message);
                return 1;
            }

            return 0;
        }
        #endregion

        #region Private Methods
        private int OnCommandLineExecute()
        {
            ExecuteArgument arg = ExecuteArgument.Create(this._appCmd.Options);

            if (arg == null)
            {
                return 1;
            }
            if (arg.Files.Count == 0)
            {
                this.Log("No files to convert.");
                return 0;
            }

            List<Document> tsDocs = this.BuildAst(arg.Files);
            if (this.ConfirmConvert(tsDocs))
            {
                this.Convert(tsDocs, arg);
            }

            return 0;
        }

        private List<Document> BuildAst(List<string> files)
        {
            List<Document> tsDocs = new List<Document>();

            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Starting build ast({0})", files.Count));

            foreach (string file in files)
            {
                AstBuilder builder = new AstBuilder();
                Document doc = builder.Build(file);

                tsDocs.Add(doc);
            }

            DateTime endTime = DateTime.Now;
            this.Log(string.Format("Finished after {0}s", (endTime - startTime).TotalSeconds.ToString("0.00")));

            return tsDocs;
        }

        private bool ConfirmConvert(List<Document> tsDocs)
        {
            this.PrintLostNodes(tsDocs);

            var nodeTypes = AstBuilder.AllNodeTypes.Keys;
            List<string> unSupportedNodes = new List<string>();

            foreach (var doc in tsDocs)
            {
                List<string> docNodeKinds = doc.GetNodeKinds();
                List<string> notImplementedNodes = docNodeKinds.FindAll((k) => !nodeTypes.Contains(k));

                foreach (var item in notImplementedNodes)
                {
                    if (!unSupportedNodes.Contains(item))
                    {
                        unSupportedNodes.Add(item);
                    }
                }
            }

            if (unSupportedNodes.Count > 0)
            {
                this.Log("Find unsupported node kinds: " + string.Join(",", unSupportedNodes) + ". Do you want continue to convert? (yes|no) ");
                string confirm = Console.ReadLine().ToLower();
                if (confirm != "y" && confirm != "yes")
                {
                    return false;
                }
            }

            return true;
        }

        private void Convert(List<Document> tsDocs, ExecuteArgument arg)
        {
            Project project = new Project(tsDocs);
            ConverterContext context = this.CreateConverterContext(arg.Config, project);
            LangConverter converter = new LangConverter(context);

            List<Document> usedDocs = this.FilterDocuments(tsDocs, context.Config.ExcludeTypes);
            
            foreach (Document doc in usedDocs)
            {
                DateTime startTime = DateTime.Now;
                this.Log(string.Format("Starting convert '{0}'", doc.FileName));

                string savePath = arg.GetSavePath(doc.Path);
                converter.Analyze(doc.Root);
                string code = converter.Convert(doc.Root);

                File.WriteAllText(savePath, code);

                DateTime endTime = DateTime.Now;
                this.Log(string.Format("Finished after {0}s", (endTime - startTime).TotalSeconds.ToString("0.00")));

                //this.PrintCode(code);
            }
        }

        private void Log(string msg, bool hasTitle = true)
        {
            if (hasTitle)
            {
                Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), msg));
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        private void PrintLostNodes(List<Document> tsDocs)
        {
            this.Log("Search node's lost child nodes");

            List<string> hasPrint = new List<string>();
            foreach (var doc in tsDocs)
            {
                var lostNodes = doc.GetLostNodes().Where((item) => hasPrint.IndexOf(item.Key) == -1).ToList();
                if (lostNodes.Count == 0)
                {
                    continue;
                }

                this.Log(doc.Path, false);

                foreach (var item in lostNodes)
                {
                    hasPrint.Add(item.Key);
                    this.Log(string.Format("  {0,-20} {1}", item.Key, item.Value), false);
                }
            }

            this.Log("Finish search");
        }

        //[Conditional("DEBUG")]
        //private void PrintCode(string code)
        //{
        //    Console.WriteLine(code);
        //}

        private ConverterContext CreateConverterContext(Config config, Project project)
        {
            CSharp.Config converterConfig = new CSharp.Config
            {
                Namespace = config.Namespace,
                PreferTypeScriptType = config.PreferTypeScriptType,
                Usings = config.Usings,
                OmittedQualifiedNames = config.OmittedQualifiedNames,
                NamespaceMappings = new Dictionary<string, string>()
            };
            foreach (string item in config.NamespaceMappings)
            {
                string[] ms = item.Split(':');
                converterConfig.NamespaceMappings[ms[0]] = ms[1];
            }

            if (config.Samples.Count > 0)
            {
                List<string> usedTypes = project.GetReferences(config.Samples.ToArray());
                List<string> allTypes = project.TypeNames;
                converterConfig.ExcludeTypes = allTypes.FindAll(t => !usedTypes.Contains(t));
            }

            return new ConverterContext(converterConfig);
        }

        private List<Document> FilterDocuments(List<Document> docs, List<string> excludedTypes)
        {
            if (excludedTypes.Count == 0)
            {
                return docs;
            }

            List<Document> usedDocs = new List<Document>();
            foreach (Document doc in docs)
            {
                bool isExcluded = doc.GetTypeNames().TrueForAll(n => excludedTypes.Contains(n));
                if (!isExcluded)
                {
                    usedDocs.Add(doc);
                }
            }
            return usedDocs;
        }

        private CommandLineApplication CreateCommand()
        {
            CommandLineApplication app = new CommandLineApplication();
            app.Name = "TsConverter";
            app.HelpOption("-?|-h|--help");

            app.Option(
               "-c|--config",
               "The config file path.",
               CommandOptionType.SingleValue);

            app.Option(
                "-e|--exclude <exclusions>",
                "Things to exclude while attacking.",
                CommandOptionType.SingleValue);

            app.Option(
                "-s|--source",
                "File or directory to convert.",
                CommandOptionType.SingleValue);

            app.Option(
                "-o|--output",
                "Directory to save the converted result.",
                CommandOptionType.SingleValue);

            return app;
        }
        #endregion
    }
}
