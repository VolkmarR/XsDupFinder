using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output.Json;

namespace XsDupFinder.Lib.Output
{
    class RenderJson : IRender
    {
        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            var data = new JsonOutput { Configuration = configuration, Duplicates = duplicates };
            RenderFileHelper.SaveRenderOutput(configuration, "duplicates.json", JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
