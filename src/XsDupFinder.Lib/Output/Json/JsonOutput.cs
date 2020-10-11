using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output.Json
{
    class JsonOutput
    {
        public int Version { get; set; } = 1;
        public Configuration Configuration { get; set; }
        public List<Duplicate> Duplicates { get; set; }

        public static JsonOutput Load(string fileName)
        {
            if (File.Exists(fileName))
                try
                { return JsonConvert.DeserializeObject<JsonOutput>(File.ReadAllText(fileName)); }
                catch
                { }

            return null;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
