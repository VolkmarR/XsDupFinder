using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Cache;
using XsDupFinder.Lib.Parser;
using Xunit;
using FluentAssertions;

namespace XsDupFinder.Tests.Cache
{
    public class CacheDBTests
    {
        string GetTempDBName() => Path.ChangeExtension(Path.GetTempFileName(), ".DB");

        CodeInfo BuildDummyCodeInfo(SourceCodeFile sourceCodeFile)
            => new CodeInfo
            {
                FileName = sourceCodeFile.FileName,
                HashCode = sourceCodeFile.HashCode,
                MethodList = new List<MethodInfo>
                    {
                        new MethodInfo
                        {
                            Name = "test", StatementList = new List<StatementInfo>
                            {
                                new StatementInfo
                                {
                                    Start = 1,
                                    End = 10,
                                    StartLine = 1, Hashcode = 999 } } } }
            };


        [Fact]
        public void NotFoundInsertFound()
        {
            var dbName = GetTempDBName();

            using (var cache = new CacheDB(dbName))
            {
                var sourceCodeFile = new SourceCodeFile("XXX", "test");

                cache.TryGetValue(sourceCodeFile, out var codeInfo).Should().Be(false);

                var newCodeInfo = BuildDummyCodeInfo(sourceCodeFile);

                cache.Add(newCodeInfo);

                cache.TryGetValue(sourceCodeFile, out codeInfo).Should().Be(true);

                codeInfo.Should().BeEquivalentTo(newCodeInfo);
            }

            File.Delete(dbName);
        }
    }
}

