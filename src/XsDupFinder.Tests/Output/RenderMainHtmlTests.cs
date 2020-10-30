using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output;
using Xunit;

namespace XsDupFinder.Tests.Output
{
    public class RenderMainHtmlTests
    {
        Configuration GetDummyConfig() => new Configuration
        {
            SourceDirectory = @"C:\Test\",
            CacheFileName = @"c:\CacheDir\Test.db",
            MinLineForDuplicate = 5,
            MinLineForFullMethodDuplicateCheck = 3,
            OutputDirectory = AppDomain.CurrentDomain.BaseDirectory,
        };

        Duplicate.Location GetDumyLocation(Configuration cfg, string fileName, string className, string methodName) => new Duplicate.Location
        {
            Filename = Path.Combine(cfg.SourceDirectory, fileName),
            ClassName = className,
            MethodName = methodName,
            StartLine = 1,
            EndLine = 5,
            PercentOfMethod = 100,
        };

        [Fact]
        public void TwoDuplicates()
        {
            var cfg = GetDummyConfig();
            var Data = new List<Duplicate>
            {
               new Duplicate
                {
                    ID=1,
                    Code = "lore ipsum 1",
                    OverlappingIDs = new List<int> { 2 },
                    Locations = new List<Duplicate.Location>
                    {
                        GetDumyLocation(cfg, "f1.prg", "C1", "M1"),
                        GetDumyLocation(cfg, "f1.prg", "C1", "M2"),
                    }
                },
                new Duplicate
                {
                    ID=2,
                    Code = "lore ipsum 2",
                    OverlappingIDs = new List<int> { 1 },
                    Locations = new List<Duplicate.Location>
                    {
                        GetDumyLocation(cfg, "f1.prg", "C1", "M1"),
                        GetDumyLocation(cfg, "f1.prg", "C1", "M2"),
                    }
                },
            };

            new RenderOutput(cfg, Data).Execute();
        }


    }
}
