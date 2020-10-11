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
    public class RenderOutput
    {
        readonly Configuration Configuration;
        readonly List<Duplicate> Duplicates;
        readonly List<IRender> Renders;

        public RenderOutput(Configuration configuration, List<Duplicate> duplicates)
        {
            Duplicates = duplicates;
            Configuration = configuration;
            Renders = new List<IRender> { new RenderJson(), new RenderMainHtml() };
        }

        public void Execute()
        {
            foreach (var render in Renders)
            {
                render.Execute(Configuration, Duplicates);
            }
        }
    }
}
