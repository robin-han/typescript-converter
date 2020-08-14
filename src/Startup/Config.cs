using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TypeScript.Converter
{
    class Config
    {
        #region Fields
        private static readonly string DEAULT_CONFIGFILE_NAME = "tscconfig.json";
        #endregion

        #region Constructor
        public Config()
        {
            this.Init();
        }

        private void Init()
        {
            this.Include = new List<string>();
            this.Exclude = new List<string>();
            this.Samples = new List<string>();
            this.Outputs = new List<Output>();
            this.NamespaceMappings = new List<string>();
            this.OmittedQualifiedNames = new List<string>();
        }
        #endregion

        #region Properties
        public List<string> Include
        {
            get;
            private set;
        }


        public List<string> Exclude
        {
            get;
            private set;
        }

        public List<string> Samples
        {
            get;
            private set;
        }

        public List<Output> Outputs
        {
            get;
            private set;
        }

        public List<string> NamespaceMappings
        {
            get;
            private set;
        }

        public List<string> OmittedQualifiedNames
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public string Read()
        {
            return Read(DEAULT_CONFIGFILE_NAME);
        }

        public string Read(string configPath)
        {
            return this.ReadConfigFile(configPath);
        }

        private string ReadConfigFile(string configFile)
        {
            if (!File.Exists(configFile))
            {
                return string.Format("Cannot find config file {0}", configFile);
            }

            JObject jsonConfig = JObject.Parse(File.ReadAllText(configFile));
            this.Init();

            //outputs
            JToken jsonOutputs = jsonConfig["outputs"];
            if (jsonOutputs != null)
            {
                this.ReadOutputs(jsonOutputs);
            }

            // namesapce mappings
            JToken jsonNSMappings = jsonConfig["namespaceMappings"];
            if (jsonNSMappings != null)
            {
                foreach (JToken item in jsonNSMappings)
                {
                    this.NamespaceMappings.Add(item.ToObject<string>());
                }
            }

            // omitted qualified names
            JToken jsonOmittedNames = jsonConfig["omittedQualifiedNames"];
            if (jsonOmittedNames != null)
            {
                foreach (JToken item in jsonOmittedNames)
                {
                    this.OmittedQualifiedNames.Add(item.ToObject<string>());
                }
            }

            // exclude
            JToken jsonExclude = jsonConfig["exclude"];
            if (jsonExclude != null)
            {
                foreach (JToken item in jsonExclude)
                {
                    this.Exclude.Add(item.ToObject<string>());
                }
            }

            // include
            JToken jsonInclude = jsonConfig["include"];
            if (jsonInclude != null)
            {
                foreach (JToken item in jsonInclude)
                {
                    this.Include.Add(item.ToObject<string>());
                }
            }

            // samples
            JToken jsonSamples = jsonConfig["samples"];
            if (jsonSamples != null)
            {
                foreach (JToken item in jsonSamples)
                {
                    this.Samples.Add(item.ToObject<string>());
                }
            }

            return string.Empty;
        }

        private void ReadOutputs(JToken jsonOutputs)
        {
            JArray jsonArray = jsonOutputs as JArray;
            if (jsonArray == null)
            {
                return;
            }

            foreach (var jsonConfig in jsonArray)
            {
                Output output = new Output();

                // path
                JToken jsonPath = jsonConfig["path"];
                if (jsonPath != null)
                {
                    output.Path = jsonPath.ToObject<string>();
                }

                //pattern
                JToken jsonPatterns = jsonConfig["patterns"];
                if (jsonPatterns != null)
                {
                    foreach (JToken item in jsonPatterns)
                    {
                        output.Patterns.Add(item.ToObject<string>());
                    }
                }

                // flatOutput
                JToken jsonFlatten = jsonConfig["flatOutput"];
                if (jsonFlatten != null)
                {
                    output.FlatOutput = jsonFlatten.ToObject<bool>();
                }

                // perfer typescript type
                JToken jsonPerferType = jsonConfig["preferTypeScriptType"];
                if (jsonPerferType != null)
                {
                    output.PreferTypeScriptType = jsonPerferType.ToObject<bool>();
                }

                // namesapce
                JToken jsonNamespace = jsonConfig["namespace"];
                if (jsonNamespace != null)
                {
                    output.Namespace = jsonNamespace.ToObject<string>();
                }

                // usings
                JToken jsonUsings = jsonConfig["usings"];
                if (jsonUsings != null)
                {
                    foreach (JToken item in jsonUsings)
                    {
                        output.Usings.Add(item.ToObject<string>());
                    }
                }

                this.Outputs.Add(output);
            }
        }
        #endregion
    }
}
