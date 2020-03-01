using Extensions.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsDupFinder.Lib.Parser
{
    public class SourceCodeFile
    {
        public string FileName { get; private set; }
        public string SourceCode { get; private set; }
        public uint HashCode { get; private set; }

        public SourceCodeFile(string fileName)
        {
            FileName = fileName;
            SourceCode = null;
            HashCode = 0;
            if (File.Exists(fileName))
            {
                SourceCode = File.ReadAllText(fileName);
                HashCode = XXHash.XXH32(Encoding.UTF8.GetBytes(SourceCode));
            }
        }
    }
}
