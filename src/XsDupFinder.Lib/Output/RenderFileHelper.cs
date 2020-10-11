using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;

namespace XsDupFinder.Lib.Output
{
    static class RenderFileHelper
    {
        static void BackupFileForTrackChanges(Configuration configuration, string fileName)
        {
            if (configuration.TrackChanges && File.Exists(fileName))
                File.Copy(fileName, fileName + ".last", true);
        }

        public static void SaveRenderOutput(Configuration configuration, string fileName, string content)
        {
            var outputFileName = Path.Combine(configuration.OutputDirectory, fileName);
            BackupFileForTrackChanges(configuration, outputFileName);
            File.WriteAllText(outputFileName, content);
        }
    }
}
