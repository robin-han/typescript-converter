using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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

        public Config Config
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default output.
        /// </summary>
        public Output Output
        {
            get
            {
                if (this.Outputs.Count > 0)
                {
                    return this.Outputs[this.Outputs.Count - 1];
                }

                return new Output();
            }
        }
        #endregion
        public Output GetOutput(string sourcePath)
        {
            string path = FileUtil.NormalizePath(sourcePath);
            foreach (var output in this.Outputs)
            {
                if (output.Patterns.Exists((pattern) =>
                {
                    return Regex.IsMatch(path, FileUtil.ToRegexPattern(FileUtil.NormalizePath(pattern)));
                }))
                {
                    return output;
                }
            }
            return this.Output;
        }

        public string GetSavePath(string sourcePath, Output output)
        {
            string outputPath = output.Path;
            if (File.Exists(outputPath))
            {
                return outputPath;
            }

            string savePath;
            if (output.FlatOutput)
            {
                savePath = Path.Combine(outputPath, Path.GetFileName(sourcePath).Split('.')[0] + ".cs");
            }
            else
            {
                string relativePath = Path.GetRelativePath(this.BasePath, sourcePath);
                if (string.IsNullOrEmpty(relativePath) || relativePath == ".")
                {
                    relativePath = Path.GetFileName(sourcePath);
                }
                savePath = Path.Combine(outputPath, relativePath.Split('.')[0] + ".cs");
            }

            string dirName = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            return savePath;
        }

        public static ExecuteArgument Create(List<CommandOption> options)
        {
            CommandOption configOption = options[1];
            CommandOption excludeOption = options[2];
            CommandOption sourceOption = options[3];
            CommandOption outOption = options[4];

            // config
            Config config = new Config();
            string errorMsg = string.Empty;
            if (configOption.HasValue())
            {
                errorMsg = config.Read(configOption.Value());
            }
            else
            {
                errorMsg = config.Read();
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Console.WriteLine(errorMsg);
                return null;
            }

            //
            string basePath = string.Empty;
            List<Output> outputs = new List<Output>();
            List<string> allFiles = new List<string>();
            List<string> exclusion = new List<string>();
            List<string> topInputs = new List<string>();
            // all files
            if (sourceOption.HasValue())
            {
                string input = sourceOption.Value();
                List<string> inputFiles = FileUtil.GetTsJsonFiles(input);
                if (inputFiles == null)
                {
                    Console.WriteLine(string.Format("Cannot find input file or directory {0}", input));
                    return null;
                }
                allFiles.AddRange(inputFiles);
                topInputs.Add(input);
            }
            if (configOption.HasValue() || !sourceOption.HasValue())
            {
                foreach (string include in config.Include)
                {
                    List<string> inputFiles = FileUtil.GetTsJsonFiles(include);
                    if (inputFiles == null)
                    {
                        Console.WriteLine(string.Format("Cannot find include file or directory {0}", include));
                        return null;
                    }
                    allFiles.AddRange(inputFiles);
                    topInputs.Add(include);
                }
            }

            // base path
            basePath = FileUtil.GetBasePath(allFiles);
            // adjust base path
            if (topInputs.Count == 1 && Directory.Exists(topInputs[0]) && basePath.StartsWith(FileUtil.NormalizePath(topInputs[0])))
            {
                basePath = topInputs[0];
            }

            // exclusion
            if (excludeOption.HasValue())
            {
                exclusion.Add(excludeOption.Value());
            }
            if (configOption.HasValue() || !excludeOption.HasValue())
            {
                exclusion.AddRange(config.Exclude);
            }

            // outputs
            if (outOption.HasValue())
            {
                outputs.Add(new Output() { Path = outOption.Value() });
            }
            if (configOption.HasValue() || !outOption.HasValue())
            {
                outputs.InsertRange(0, config.Outputs);
            }

            // files
            List<string> files = FileUtil.FilterFiles(allFiles, exclusion);

            return new ExecuteArgument()
            {
                AllFiles = allFiles,
                Files = files,
                Outputs = outputs,
                BasePath = basePath,

                Config = config
            };
        }
    }


}
