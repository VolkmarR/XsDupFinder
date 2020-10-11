using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace XsDupFinder.Lib.Output.Json
{
    class JsonChangesItem
    {
        public DateTime Changed { get; set; }
        public bool ConfigurationChanged { get; set; }
        public int NumberOfFragements { get; set; }
        public int NumberOfLocations { get; set; }
    }
}
