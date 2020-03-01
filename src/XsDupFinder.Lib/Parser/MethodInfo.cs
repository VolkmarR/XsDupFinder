using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsDupFinder.Lib.Parser
{
    class MethodInfo
    {
        public string Name { get; set; }
        public List<StatementInfo> StatementList { get; set; } = new List<StatementInfo>();
    }
}
