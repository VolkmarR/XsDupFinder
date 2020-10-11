using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;

namespace XsDupFinder.Lib.Output.Renderer
{
    static class RenderFileHelper
    {
        public static string BuildOutputFileName(Configuration configuration, string fileName)
            => Path.Combine(configuration.OutputDirectory, fileName);

        public static void SaveRenderOutput(Configuration configuration, string fileName, string content)
        {
            var outputFileName = BuildOutputFileName(configuration, fileName);
            File.WriteAllText(outputFileName, content);
        }
    }
}
