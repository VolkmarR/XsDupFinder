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

        public SourceCodeFile(string fileName) : this(fileName, File.Exists(fileName) ? File.ReadAllText(fileName) : "")
        { }

        public SourceCodeFile(string fileName, string sourceCode)
        {
            FileName = fileName;
            SourceCode = null;
            SourceCode = sourceCode;
            HashCode = XXHash.XXH32(Encoding.UTF8.GetBytes(SourceCode));
        }
    }
}
