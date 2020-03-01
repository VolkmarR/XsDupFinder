using Extensions.Data;
using System.Text;

namespace XsDupFinder.Lib.Parser
{
    public class StatementInfo
    {
        public uint Hashcode { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int StartLine { get; set; }

        public StatementInfo()
        { }

        public StatementInfo(string statement, int start, int end, int startLine)
        {
            Hashcode = XXHash.XXH32(Encoding.UTF8.GetBytes(statement));
            Start = start;
            End = end;
            StartLine = startLine;
        }

    }
}
