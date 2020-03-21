using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsDupFinder.Lib.Finder
{
    public class Duplicate
    {
        public class Location
        {
            public string Filename { get; set; }
            public string MethodName { get; set; }
            public int StartLine { get; set; }
            public int EndLine { get; set; }
            public int PercentOfMethod { get; set; }
            public bool IsFullMethod { get; set; }
        }

        public int ID { get; set; }
        public List<int> OverlappingIDs { get; set; }
        public int LineCount => Locations.Count > 0 ? Locations[0].EndLine - Locations[0].StartLine + 1 : 0;
        public string Code { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
    }
}
