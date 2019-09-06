using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter
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

        public string Output
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
        #endregion

        public string GetSavePath(string sourcePath)
        {
            string basePath = this.BasePath;
            string output = this.Output;

            if (File.Exists(output))
            {
                return output;
            }

            string path;
            if (string.IsNullOrEmpty(basePath))
            {
                path = Path.Combine(output, Path.GetFileNameWithoutExtension(sourcePath).Split('.')[0] + ".cs");
            }
            else
            {
                string relativePath = Path.GetRelativePath(basePath, sourcePath);
                if (string.IsNullOrEmpty(relativePath) || relativePath == ".")
                {
                    relativePath = Path.GetFileName(sourcePath);
                }
                path = Path.Combine(output, relativePath.Split('.')[0] + ".cs");
            }

            string dirName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            return path;
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
            string output = string.Empty;
            List<string> allFiles = new List<string>();
            List<string> exclusion = new List<string>();

            //input source
            if (sourceOption.HasValue())
            {
                string input = sourceOption.Value();
                List<string> inputFiles = Utils.GetTsJsonFiles(input);
                if (inputFiles == null)
                {
                    Console.WriteLine(string.Format("Cannot find input file or directory {0}", input));
                    return null;
                }
                allFiles.AddRange(inputFiles);
            }
            if (configOption.HasValue() || !sourceOption.HasValue())
            {
                allFiles.AddRange(config.Include);
            }
            //base path
            if (!config.FlatOutput)
            {
                basePath = Utils.GetBasePath(allFiles);
            }

            //exclusion
            if (excludeOption.HasValue())
            {
                exclusion.Add(excludeOption.Value());
            }
            if (configOption.HasValue() || !excludeOption.HasValue())
            {
                exclusion.AddRange(config.Exclude);
            }

            //output
            output = outOption.HasValue() ? outOption.Value() : config.Output;

            //
            List<string> files = Utils.FilterFiles(allFiles, exclusion);

            return new ExecuteArgument()
            {
                AllFiles = allFiles,
                Files = files,
                Output = output,
                BasePath = basePath,

                Config = config
            };
        }
    }
}
