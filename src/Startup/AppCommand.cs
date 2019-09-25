using TypeScript.Converter.CSharp;
using TypeScript.Syntax;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TypeScript.Converter
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
            this._appCmd.OnExecute(new Func<int>(this.OnCommandExecute));
        }
        #endregion

        #region Command Execute
        /// <summary>
        /// Executes the application command.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        /// <returns>0 success, otherwise failure.</returns>
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

        /// <summary>
        /// Command execute handler.
        /// </summary>
        /// <returns>0 success, otherwise failure.</returns>
        private int OnCommandExecute()
        {
            ExecuteArgument arg = ExecuteArgument.Create(this._appCmd.Options);

            if (arg == null)
            {
                return 1;
            }
            if (arg.Files.Count == 0 && arg.Config.Samples.Count == 0)
            {
                this.Log("No files to convert.");
                return 0;
            }

            List<Document> documents = this.BuildAst(arg.AllFiles);
            List<Document> includedDocuments = (arg.Files == arg.AllFiles ? documents : documents.FindAll(doc => arg.Files.Contains(doc.Path)));
            if (this.ConfirmConvert(includedDocuments))
            {
                Project project = new Project(arg.BasePath, documents, includedDocuments);
                ConverterContext context = this.CreateConverterContext(project, arg.Config);
                this.Convert(context, arg);
            }
            return 0;
        }

        /// <summary>
        /// Creates command hints.
        /// </summary>
        /// <returns></returns>
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

        #region Private Methods
        /// <summary>
        /// Build typescript ast.
        /// </summary>
        /// <param name="files">The typescript json files.</param>
        /// <returns>The ast document list</returns>
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

            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));
            return tsDocs;
        }

        /// <summary>
        /// Search lost and unsupport syntax node.
        /// </summary>
        /// <param name="tsDocs">The ast documents.</param>
        /// <returns></returns>
        private bool ConfirmConvert(List<Document> tsDocs)
        {
            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Starting search lost nodes and find not implement node types."));

            this.PrintLostNodes(tsDocs);
            List<string> unSupportedNodes = DocumentUtil.GetNotImplementNodeTypes(tsDocs);

            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            //
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

        /// <summary>
        /// Convert typescript to csharp code.
        /// </summary>
        /// <param name="context">The convert context.</param>
        /// <param name="arg">Execute argument.</param>
        private void Convert(ConverterContext context, ExecuteArgument arg)
        {
            List<Document> avaiableDocs = this.FilterDocuments(context.Project.IncludedDocuments, context.Config.ExcludeTypes);

            // analyze
            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Starting analyze files({0})", avaiableDocs.Count));
            List<Node> nodes = new List<Node>();
            foreach (Document doc in avaiableDocs)
            {
                nodes.Add(doc.Root);
            }
            context.Analyze(nodes);
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            // convert
            startTime = DateTime.Now;
            this.Log(string.Format("Starting convert files({0})", avaiableDocs.Count));
            foreach (Document doc in avaiableDocs)
            {
                DateTime beginTime = DateTime.Now;
                this.Log(string.Format("Starting convert file '{0}'", Path.GetFileNameWithoutExtension(doc.Path)));

                string savePath = arg.GetSavePath(doc.Path);
                string code = doc.Root.ToCSharp();
                File.WriteAllText(savePath, code);

                this.Log(string.Format("Finished after {0}s", (DateTime.Now - beginTime).TotalSeconds.ToString("0.00")));
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));
        }

        /// <summary>
        /// Creates convert context.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="config">The convert config.</param>
        /// <returns>Convert context.</returns>
        private ConverterContext CreateConverterContext(Project project, Config config)
        {
            ConverterConfig converterConfig = new ConverterConfig
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
                //always convert samples
                foreach (var sample in config.Samples)
                {
                    Document doc = project.GetDocumentByType(sample);
                    if (doc != null)
                    {
                        project.AddConversionDocument(doc);
                    }
                }
                List<string> avaiableTypes = project.GetReferences(config.Samples.ToArray());
                List<string> allTypes = project.TypeNames;
                converterConfig.ExcludeTypes = allTypes.FindAll(t => !avaiableTypes.Contains(t));
            }

            ConverterContext convertContext = new ConverterContext(project, converterConfig);
            ConverterContext.Current = convertContext;
            return convertContext;
        }

        /// <summary>
        /// Filter documents.
        /// </summary>
        /// <param name="docs">All documents.</param>
        /// <param name="excludedTypes">The exclude types.</param>
        /// <returns>Avaiable documents.</returns>
        private List<Document> FilterDocuments(List<Document> docs, List<string> excludedTypes)
        {
            if (excludedTypes.Count == 0)
            {
                return docs;
            }

            List<Document> usedDocs = new List<Document>();
            foreach (Document doc in docs)
            {
                bool isExcluded = DocumentUtil.GetTypeNames(doc).TrueForAll(n => excludedTypes.Contains(n));
                if (!isExcluded)
                {
                    usedDocs.Add(doc);
                }
            }
            return usedDocs;
        }

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="hasTime">Indicate whether add time on the head of message.</param>
        private void Log(string msg, bool hasTime = true)
        {
            if (hasTime)
            {
                Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), msg));
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// Prints lost nodes.
        /// </summary>
        /// <param name="tsDocs"></param>
        private void PrintLostNodes(List<Document> tsDocs)
        {
            List<string> hasPrint = new List<string>();
            foreach (var doc in tsDocs)
            {
                var lostNodes = DocumentUtil.GetLostNodes(doc).Where((item) => hasPrint.IndexOf(item.Key) == -1).ToList();
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
        }
        #endregion
    }
}
