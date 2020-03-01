using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Parser;

namespace XsDupFinderCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            new MethodExtractor().Execute(@"..\..\..\..\assets\TestData\simpleFile.prg ");
            Console.ReadKey();
        }
    }
}
