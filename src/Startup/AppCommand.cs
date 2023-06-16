using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.CommandLineUtils;

using TypeScript.Converter;
using TypeScript.Converter.CSharp;
using TypeScript.Converter.Java;
using TypeScript.Syntax;
using TypeScript.Syntax.Converter;
using GrapeCity.Syntax.Converter.Console.Exceptions;
using GrapeCity.Syntax.Converter.Source.TypeScript.Builders;

namespace GrapeCity.Syntax.Converter.Console
{
    class ConverterApplication : CommandLineApplication
    {
        private const string defaultConfigFilePath = "./tscconfig.json";

        public ConverterApplication()
        {
            this.Name = "SyntaxConverter";
            this.FullName = "The source code converter from source code to a specified programming language";
            this.Description = "This ia a converter from source code to a specified programming language";

            this.HelpOption("-h|--help");
            var configFilePathOption = this.Option("-c|--config <filePath>", "the config file with json format. The default value is \"config.json\"", CommandOptionType.SingleValue);

            this.OnExecute(() =>
            {
                if (configFilePathOption.Values.Count == 0)
                {
                    configFilePathOption.Values.Add(defaultConfigFilePath);
                }
                var configFilePath = Path.GetFullPath(configFilePathOption.Value());
                if (!File.Exists(configFilePath))
                {
                    throw new ConfigFileNotFindException(configFilePath);
                }

                var arg = ExecuteArgument.Create(configFilePath);

                if (arg == null)
                {
                    return 1;
                }
                if (arg.Files.Count == 0 && arg.Config.Samples.Count == 0)
                {
                    this.Log("No files to convert.");
                    return 0;
                }

                Project project = this.CreateProject(arg);
                var documents = project.SampleDocuments;

                if (this.ConfirmConvert(documents))
                {
                    this.ClearOutput(project, arg);

                    IConverter converter = this.CreateConverter(project, arg);
                    project.Converter = converter;
                    List<Document> translateDocs = this.FilterDocuments(documents, converter.Context.ExcludeTypes);
                    this.Convert(converter, arg, translateDocs);
                }
                return 0;
            });
        }

        /// <summary>
        /// Build typescript ast.
        /// </summary>
        /// <param name="files">The typescript json files.</param>
        /// <returns>The ast document list</returns>
        private List<Document> BuildAst(List<string> files)
        {
            AnalysisVisitor visitor = new AnalysisVisitor();
            List<Document> documents = new List<Document>();

            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Start building abstract syntax tree..."));
            var builder = new AbstractSyntaxTreeBuilder();
            foreach (string file in files)
            {
                documents.Add(builder.Build(file, visitor));
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            startTime = DateTime.Now;
            this.Log(string.Format("Start validating unexpected semicolon..."));
            var unexpectedSemicolonValidator = new UnexpectedSemicolonValidator();
            var errors = unexpectedSemicolonValidator.Validate(visitor.Classes.ToArray());
            if (errors.Count() > 0)
            {
                foreach (var error in errors)
                {
                    this.Log(error.Message);
                }
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            startTime = DateTime.Now;
            this.Log(string.Format("Start validating unexpected member syntax..."));
            var unexpectedMemberSyntaxValidator = new UnexpectedMemberSyntaxValidator();
            errors = unexpectedMemberSyntaxValidator.Validate(visitor.Classes.ToArray());
            if (errors.Count() > 0)
            {
                foreach (var error in errors)
                {
                    this.Log(error.Message);
                }
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            return documents;
        }

        /// <summary>
        /// Search lost and unsupport syntax node.
        /// </summary>
        /// <param name="tsDocs">The ast documents.</param>
        /// <returns></returns>
        private bool ConfirmConvert(List<Document> tsDocs)
        {
            // Defined Properties
            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Starting search defined properties in node types."));
            var notDefinedProperties = this.GetNotDefinedProperties(tsDocs);
            if (notDefinedProperties.Count > 0)
            {
                this.Log("Find none defined properties:");
                foreach (var item in notDefinedProperties)
                {
                    this.Log(item.Key, false);
                    foreach (var v in item.Value)
                    {
                        this.Log(string.Format("  {0,-20} {1}", v.Key, v.Value), false);
                    }
                }
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            // Supported Node Types
            List<string> unSupportedNodes = DocumentUtil.GetNotImplementNodeTypes(tsDocs);
            if (unSupportedNodes.Count > 0)
            {
                this.Log("Find unsupported node kinds:");
                this.Log("  " + string.Join(Environment.NewLine + " ", unSupportedNodes), false);
                this.Log("Do you want continue? (yes|no)", false);

                string confirm = System.Console.ReadLine().ToLower();
                if (confirm != "y" && confirm != "yes")
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clear output directory's files which not in input files.
        /// </summary>
        private void ClearOutput(Project project, ExecuteArgument arg)
        {
            Output output = arg.Outputs.Find(o => o.Patterns.Count == 0);
            output = output ?? new Output();

            if (!Directory.Exists(output.Path) || !Directory.Exists(arg.BasePath))
            {
                return;
            }

            List<string> deleteFiles = new List<string>();
            //arg.AllFiles;
            List<string> allInputSaveFiles = project.Documents.Select(doc =>
            {
                string savePath = arg.GetSavePath(doc, output);
                return Path.GetRelativePath(output.Path, savePath);
            }).ToList();
            var inputDirectories = Directory.GetDirectories(arg.BasePath);
            var outputDirectories = Directory.GetDirectories(output.Path).Where(dir1 =>
            {
                return inputDirectories.Any(dir2 => Path.GetFileName(dir2) == Path.GetFileName(dir1));
            });
            string fileExtension = FileUtil.GetFileExtension(arg.OutputLang);
            foreach (string dir in outputDirectories)
            {
                var existOutputFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(fileExtension))
                    .Select(s => Path.GetRelativePath(dir, s));

                string dirName = Path.GetFileName(dir);
                foreach (string file in existOutputFiles)
                {
                    if (!allInputSaveFiles.Contains(Path.Combine(dirName, file)))
                    {
                        deleteFiles.Add(Path.Combine(dir, file));
                    }
                }
            }

            foreach (string file in deleteFiles)
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Convert typescript to csharp code.
        /// </summary>
        /// <param name="context">The convert context.</param>
        /// <param name="arg">Execute argument.</param>
        private void Convert(IConverter converter, ExecuteArgument arg, List<Document> documents)
        {
            ConvertContext context = (ConvertContext)converter.Context;
            IProject project = context.Project;

            // normalize documents
            DateTime startTime = DateTime.Now;
            this.Log(string.Format("Starting analyze files({0})", documents.Count));
            project.Normalize(documents);
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));

            // convert
            startTime = DateTime.Now;
            this.Log(string.Format("Starting convert files({0})", documents.Count));
            foreach (Document doc in documents)
            {
                DateTime beginTime = DateTime.Now;
                this.Log(string.Format("Starting convert file {0}", Path.GetFileNameWithoutExtension(doc.Path)));

                Output output = arg.GetOutput(doc);
                if (output == null)
                {
                    this.LogWarning("Cannot get output path in config and skip it");
                    continue;
                }

                // modify current convert context
                context.Namespace = output.Namespace;
                context.Usings = output.Usings;
                context.TypeScriptType = output.TypeScriptType;
                context.TypeScriptAdvancedType = output.TypeScriptAdvancedType;
                string savePath = arg.GetSavePath(doc, output);
                // Create directory
                string dirName = Path.GetDirectoryName(savePath);
                if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                string code = converter.Convert(doc.Source);
                File.WriteAllText(savePath, code);

                //
                this.Log(string.Format("Finished after {0}s", (DateTime.Now - beginTime).TotalSeconds.ToString("0.00")));
            }
            this.Log(string.Format("Finished after {0}s", (DateTime.Now - startTime).TotalSeconds.ToString("0.00")));
        }

        /// <summary>
        /// Creates the project
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Project CreateProject(ExecuteArgument arg)
        {
            List<Document> documents = this.BuildAst(arg.AllFiles);
            List<Document> sampleDocuments = documents.FindAll(doc => arg.Files.Contains(doc.Path));

            Project project = new Project(arg.BasePath, documents)
            {
                GroupId = arg.Config.GroupId
            };

            //always convert samples
            foreach (var sample in arg.Config.Samples)
            {
                Document doc = project.GetTypeDeclarationDocument(sample);
                if (doc != null && !sampleDocuments.Contains(doc))
                {
                    sampleDocuments.Add(doc);
                }
            }
            project.SampleDocuments = sampleDocuments;

            return project;
        }

        /// <summary>
        /// Creates convert context.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="config">The convert config.</param>
        /// <returns>Convert context.</returns>
        private IConverter CreateConverter(Project project, ExecuteArgument arg)
        {
            // create context
            ConvertContext context = new ConvertContext(project, arg);
            context.QualifiedNames = arg.Config.QualifiedNames;

            if (arg.Config.Samples.Count > 0)
            {
                List<string> avaiableTypes = project.GetReferences(arg.Config.Samples);
                List<string> allTypes = project.TypeDeclarationNames;
                context.ExcludeTypes = allTypes.FindAll(t => !avaiableTypes.Contains(t));
            }

            switch (arg.OutputLang)
            {
                case Lang.CSharp:
                    return new CSharpConverter(context);

                case Lang.Java:
                    return new JavaConverter(context);

                default:
                    return null;
            }
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
                bool isExcluded = doc.TypeDeclarationNames.TrueForAll(n => excludedTypes.Contains(n));
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
                System.Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {msg}");
            }
            else
            {
                System.Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="hasTime">Indicate whether add time on the head of message.</param>
        private void LogWarning(string msg, bool hasTime = true)
        {
            if (hasTime)
            {
                System.Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Warning: {msg}");
            }
            else
            {
                System.Console.WriteLine($"Warning: {msg}");
            }
        }

        /// <summary>
        /// Gets node not defined properties.
        /// </summary>
        /// <param name="tsDocs"></param>
        private Dictionary<string, IEnumerable<KeyValuePair<string, string>>> GetNotDefinedProperties(List<Document> tsDocs)
        {
            //Doc: [NodeKind: Property]
            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> ret = new Dictionary<string, IEnumerable<KeyValuePair<string, string>>>();

            HashSet<string> hasFounds = new HashSet<string>();
            foreach (var doc in tsDocs)
            {
                var lostNodes = DocumentUtil.GetLostNodes(doc).Where((item) => !hasFounds.Contains(item.Key));
                if (lostNodes.Count() > 0)
                {
                    foreach (var lost in lostNodes)
                    {
                        hasFounds.Add(lost.Key);
                    }
                    ret.Add(doc.Path, lostNodes);
                }
            }

            return ret;
        }
    }
}
