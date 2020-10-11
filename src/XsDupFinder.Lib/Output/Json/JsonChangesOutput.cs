using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XsDupFinder.Lib.Output.Json
{
    class JsonChangesOutput
    {
        public List<JsonChangesItem> Items { get; set; } = new List<JsonChangesItem>();

        public static JsonChangesOutput Load(string fileName)
        {
            if (File.Exists(fileName))
                try
                { return JsonConvert.DeserializeObject<JsonChangesOutput>(File.ReadAllText(fileName)); }
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
