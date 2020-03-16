using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;

namespace XsDupFinderCmd
{
    class ConfigurationCmd : Configuration
    {
        [Option('c', "ConfgFile", SetName = "Config File", HelpText = "Configuration file", Required = true)]
        public string ConfgFile { get; set; }
    }
}
