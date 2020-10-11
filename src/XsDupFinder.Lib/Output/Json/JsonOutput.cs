using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output.Json
{
    class JsonOutput
    {
        public int Version { get; set; } = 1;
        public Configuration Configuration { get; set; }
        public List<Duplicate> Duplicates { get; set; }
    }
}
