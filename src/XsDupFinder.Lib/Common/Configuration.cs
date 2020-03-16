using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace XsDupFinder.Lib.Common
{
    public class Configuration
    {
        [Option('s', "SourceDirectory", SetName = "Config", HelpText = "Directory where the sourcecode files are located", Required = true)]
        public string SourceDirectory { get; set; }
        [Option('t', "CacheFileName", SetName = "Config", HelpText = "Filename of the cache data file")]
        public string CacheFileName { get; set; }
        [Option('o', "OutputDirectory", SetName = "Config", HelpText = "Directory for the output files")]
        public string OutputDirectory { get; set; }
        [Option('m', "MinLineForDuplicate", SetName = "Config", HelpText = "Minimum number of equal lines needed to count as duplicate code block", Default = 15)]
        public int MinLineForDuplicate { get; set; } = 5;
        [Option('f', "MinLineForDuplMethodCheck", SetName = "Config", HelpText = "Minimum number of lines needed to qualify for the duplicate method check", Default = 3)]
        public int MinLineForFullMethodDuplicateCheck { get; set; } = 5;

        public Configuration FixOptionalValues()
        {
            if (string.IsNullOrWhiteSpace(SourceDirectory))
                return this;

            if (MinLineForDuplicate < 5)
                MinLineForDuplicate = 5;

            if (MinLineForFullMethodDuplicateCheck < 5)
                MinLineForFullMethodDuplicateCheck = 5;

            if (!SourceDirectory.EndsWith(@"\"))
                SourceDirectory += @"\";

            if (string.IsNullOrEmpty(CacheFileName))
                CacheFileName = Path.Combine(SourceDirectory, "XsDupFinderCache.db");
            if (string.IsNullOrEmpty(OutputDirectory))
                OutputDirectory = SourceDirectory;

            return this;
        }

        public static Configuration Load(string filename)
        {
            return new Deserializer().Deserialize<Configuration>(File.ReadAllText(filename));
        }

        public void SaveConfig(string filename)
        {
            File.WriteAllText(filename, new Serializer().Serialize(this));
        }
    }
}