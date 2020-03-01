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
            var xx = new MethodExtractor().Execute(new SourceCodeFile(@"..\..\..\..\assets\TestData\simpleFile.prg "));
            foreach (var methodInfo in xx.MethodList)
            {
                Console.WriteLine(methodInfo.Name);
            }


            Console.ReadKey();
        }
    }
}
