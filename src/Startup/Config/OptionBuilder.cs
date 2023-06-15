using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using GrapeCity.Syntax.Converter.Console.Exceptions;

namespace TypeScript.Converter
{
    internal class OptionBuilder
    {
        public ConfigOption Read(string configFile)
        {
            if (!File.Exists(configFile))
            {
                throw new ConfigFileNotFindException(Path.GetFullPath(configFile));
            }

            var option = new ConfigOption();

            string configPath = Path.GetFullPath(configFile);
            var config = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            // target
            var target = config.GetSection("target");
            if (target != null)
            {
                this.ReadTarget(option, target, configPath);
            }

            // omitted qualified names
            var qualifiedNames = config.GetSection("qualifiedNames");
            if (qualifiedNames != null)
            {
                foreach (var item in qualifiedNames.GetChildren())
                {
                    option.QualifiedNames.Add(item.Value);
                }
            }

            // exclude
            var exclude = config.GetSection("exclude");
            if (exclude != null)
            {
                foreach (var item in exclude.GetChildren())
                {
                    option.Exclude.Add(item.Value);
                }
            }

            // source
            var source = config.GetSection("source");
            if (source != null)
            {
                string configDir = Path.GetDirectoryName(configPath);
                foreach (var item in source.GetChildren())
                {
                    string src = item.Value;
                    if (!Path.IsPathRooted(src))
                    {
                        src = Path.Combine(configDir, src);
                    }
                    option.AstFolderPaths.Add(src);
                }
            }

            // samples
            var samples = config.GetSection("samples");
            if (samples != null)
            {
                foreach (var item in samples.AsEnumerable(true))
                {
                    option.Samples.Add(item.Value);
                }
            }

            return option;
        }

        private void ReadTarget(ConfigOption option, IConfigurationSection targetConfig, string configPath)
        {
            string configDir = Path.GetDirectoryName(configPath);

            option.GroupId = targetConfig["groupId"];

            var lang = targetConfig["lang"];
            if (!string.IsNullOrEmpty(lang))
            {
                option.OuputLang = ParseLang(lang);
            }

            var outputs = targetConfig.GetSection("outputs");
            if (outputs == null)
            {
                return;
            }
            foreach (var outputConfig in outputs.GetChildren())
            {
                Output output = new Output();


                // path
                var path = outputConfig["path"];
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(configDir, path);
                }
                output.Path = path;

                //pattern
                var patterns = outputConfig.GetSection("patterns");
                if (patterns != null)
                {
                    foreach (var item in patterns.GetChildren())
                    {
                        output.Patterns.Add(item.Value);
                    }
                }

                // flatOutput
                var flatOutput = outputConfig["flat"];
                if (!string.IsNullOrEmpty(flatOutput))
                {
                    output.Flat = bool.Parse(flatOutput);
                }

                // prefer typescript type
                var typeScriptType = outputConfig["typeScriptType"];
                if (!string.IsNullOrEmpty(typeScriptType))
                {
                    output.TypeScriptType = bool.Parse(typeScriptType);
                }

                // prefer advanced typescript types (Intersection/Union)
                var typeScriptAdvancedType = outputConfig["typeScriptAdvancedType"];
                if (!string.IsNullOrEmpty(typeScriptAdvancedType))
                {
                    output.TypeScriptAdvancedType = bool.Parse(typeScriptAdvancedType);
                }

                // namesapce
                output.Namespace = outputConfig["namespace"];

                // usings
                var usings = outputConfig.GetSection("usings");
                if (usings != null)
                {
                    foreach (var item in usings.GetChildren())
                    {
                        output.Usings.Add(item.Value);
                    }
                }

                option.Outputs.Add(output);
            }
        }

        public static Lang ParseLang(string value)
        {
            return (Lang)Enum.Parse(typeof(Lang), value, true);
        }
    }
}
