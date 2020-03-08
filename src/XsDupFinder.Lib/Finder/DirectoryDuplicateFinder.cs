using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Cache;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Parser;

namespace XsDupFinder.Lib.Finder
{
    public class DirectoryDuplicateFinder
    {
        readonly Action<string> ProcessUpdate = null;
        readonly Configuration Configuration = null;

        public DirectoryDuplicateFinder(Configuration configuration, Action<string> processUpdate = null)
        {
            ProcessUpdate = processUpdate;
            Configuration = configuration;
        }

        public List<Duplicate> Execute()
        {
            ProcessUpdate?.Invoke($"Updating Cache ({Configuration.CacheFileName})");

            var cacheDB = new CacheDB(Configuration.CacheFileName);
            var methodExtractor = new MethodExtractor();
            var finder = new DuplicateFinder(Configuration);

            var files = Directory.EnumerateFiles(Configuration.SourceDirectory, "*.prg", SearchOption.AllDirectories).ToList();
            ProcessUpdate?.Invoke($"Found {files.Count} source files");

            var index = 1;
            foreach (var fileName in files)
            {
                var sourceCodeFile = new SourceCodeFile(fileName);

                ProcessUpdate?.Invoke($"[{index++} of {files.Count}] {sourceCodeFile.RelativeFileName(Configuration.SourceDirectory)}");

                try
                {
                    if (!cacheDB.TryGetValue(sourceCodeFile, out var codeInfo))
                    {
                        codeInfo = methodExtractor.Execute(sourceCodeFile);
                        cacheDB.Add(codeInfo);
                    }
                    finder.AddSourceCodeFile(sourceCodeFile, codeInfo);
                }
                catch
                {
                    ProcessUpdate?.Invoke("File could not be parsed and will be skipped");
                }
            }

            ProcessUpdate?.Invoke("Identifying duplicates");
            var result = finder.Execute();
            ProcessUpdate?.Invoke($"{(result.Count > 0 ? result.Count.ToString() : "No")} duplicates found");

            return result;
        }
    }
}
