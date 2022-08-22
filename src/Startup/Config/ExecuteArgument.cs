using System;
using System.Collections.Generic;
using System.IO;

using TypeScript.Syntax;

namespace TypeScript.Converter
{
    class ExecuteArgument
    {
        private ExecuteArgument()
        {
        }

        #region Properties
        public List<string> AllFiles
        {
            get;
            private set;
        }

        public List<string> Files
        {
            get;
            private set;
        }

        public List<Output> Outputs
        {
            get;
            private set;
        }

        public string BasePath
        {
            get;
            private set;
        }

        public ConfigOption Config
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the output language.
        /// </summary>
        public Lang OutputLang
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public Output GetOutput(Document doc)
        {
            foreach (var output in this.Outputs)
            {
                if (output.IsMatch(doc))
                {
                    return output;
                }
            }
            //Not set pattern output
            return this.Outputs.Find(output => output.Patterns.Count == 0);
        }

        public string GetSavePath(Document doc, Output output)
        {
            string docPath = doc.Path;
            string outputPath = output.Path;
            if (File.Exists(outputPath))
            {
                return outputPath;
            }

            string extension = FileUtil.GetFileExtension(OutputLang);
            string path;
            if (output.Flat)
            {
                path = Path.Combine(outputPath, Path.GetFileName(docPath).Split('.')[0] + extension);
            }
            else
            {
                string relativePath = Path.GetRelativePath(this.BasePath, docPath);
                if (string.IsNullOrEmpty(relativePath) || relativePath == ".") // no relative
                {
                    relativePath = Path.GetFileName(docPath);
                }
                path = Path.Combine(outputPath, relativePath.Split('.')[0] + extension);
            }

            string dir = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileName = FileUtil.NormalizeName(doc, fileName, OutputLang);
            return Path.Combine(dir, fileName + extension);
        }

        public static ExecuteArgument Create(string configFilePath)
        {
            var config = new OptionBuilder().Read(configFilePath);

            List<string> astFolderPaths = new List<string>();
            List<string> astFilePaths = new List<string>();
            foreach (string astFolderPath in config.AstFolderPaths)
            {
                List<string> inputFiles = FileUtil.GetTsJsonFiles(astFolderPath);
                if (inputFiles == null)
                {
                    continue;
                }
                astFilePaths.AddRange(inputFiles);
                astFolderPaths.Add(astFolderPath);
            }

            string basePath;
            basePath = FileUtil.GetBasePath(astFilePaths);
            if (astFolderPaths.Count == 1 && Directory.Exists(astFolderPaths[0]) && basePath.StartsWith(FileUtil.NormalizePath(astFolderPaths[0])))
            {
                basePath = astFolderPaths[0];
            }

            List<string> exclusion = new List<string>();
            exclusion.AddRange(config.Exclude);

            List<Output> outputs = new List<Output>();
            outputs.InsertRange(0, config.Outputs);

            var outputLang = config.OuputLang;

            // files
            List<string> files = FileUtil.FilterFiles(astFilePaths, exclusion);

            return new ExecuteArgument()
            {
                AllFiles = astFilePaths,
                Files = files,
                Outputs = outputs,
                OutputLang = outputLang,
                BasePath = basePath,

                Config = config
            };
        }
        #endregion
    }

}
