using System;
using System.Collections.Generic;
using System.Linq;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output.Json;

namespace XsDupFinder.Lib.Output.Renderer
{
    class RenderJson : IRender
    {
        public const string FileName = "duplicates.json";

        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            var data = new JsonOutput { Configuration = configuration, Duplicates = duplicates };
            RenderFileHelper.SaveRenderOutput(configuration, FileName, data.ToJsonString());
        }
    }
}
