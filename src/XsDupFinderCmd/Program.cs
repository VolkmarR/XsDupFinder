using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output;
using CommandLine;
using System.IO;

namespace XsDupFinderCmd
{
    class Program
    {
        static void Execute(Configuration configuration)
        {
            configuration.FixOptionalValues();
            var duplicates = new DirectoryDuplicateFinder(configuration, (msg) => Console.WriteLine(msg)).Execute();
            new RenderOutput(configuration, duplicates).Execute();
        }

        static void AnalyzeArguments(ConfigurationCmd configuration)
        {
            if (string.IsNullOrEmpty(configuration.ConfgFile))
                Execute(configuration);
            else
            {
                if (!File.Exists(configuration.ConfgFile))
                {
                    Console.WriteLine($"Configuration file {configuration.ConfgFile} not found. Dummy file created.");
                    new Configuration().SaveConfig(configuration.ConfgFile);
                }
                else
                    Execute(Configuration.Load(configuration.ConfgFile));
            }
        }
        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<ConfigurationCmd>(args)
                .WithParsed<ConfigurationCmd>(opts => AnalyzeArguments(opts));
        }
    }
}
