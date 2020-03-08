using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output;
using CommandLine;

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

        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<Configuration>(args)
                .WithParsed<Configuration>(opts => Execute(opts));
        }
    }
}
