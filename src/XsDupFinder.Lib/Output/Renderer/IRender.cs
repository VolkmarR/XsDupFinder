using System;
using System.Collections.Generic;
using System.Linq;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output.Renderer
{
    interface IRender
    {
        void Execute(Configuration configuration, List<Duplicate> duplicates);
    }
}
