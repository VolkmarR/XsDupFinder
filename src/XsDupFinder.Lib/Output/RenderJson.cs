using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output
{
    class RenderJson : IRender
    {
        public void Execute(Configuration configuration, List<Duplicate> duplicates)
            => RenderFileHelper.SaveRenderOutput(configuration, "duplicates.json", JsonConvert.SerializeObject(duplicates, Formatting.Indented));
    }
}
