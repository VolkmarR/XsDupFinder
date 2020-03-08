using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output
{
    class RenderJson : IRender
    {
        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            var fileName = Path.Combine(configuration.OutputDirectory, "duplicates.json");
            File.WriteAllText(fileName, JsonConvert.SerializeObject(duplicates, Formatting.Indented));
        }
    }
}
