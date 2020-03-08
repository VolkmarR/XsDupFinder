using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output
{
    interface IRender
    {
        void Execute(Configuration configuration, List<Duplicate> duplicates);
    }
}
