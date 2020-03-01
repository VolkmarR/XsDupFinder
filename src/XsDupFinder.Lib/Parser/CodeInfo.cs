using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsDupFinder.Lib.Parser
{
    class CodeInfo
    {
        public string FileName { get; set; }
        public uint HashCode { get; set; }
        public List<MethodInfo> MethodList { get; set; } = new List<MethodInfo>();
    }
}
