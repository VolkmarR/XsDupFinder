using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Parser;
using Xunit;

namespace XsDupFinder.Tests.Parser
{
    public class MethodExtractorTests
    {
        [Fact]
        public void SimpleFileMethodNames()
        {
            var codeInfo = new MethodExtractor().Execute(new SourceCodeFile(@"..\..\..\..\assets\TestData\simpleFile.prg"));
            codeInfo.MethodList.Should().HaveCount(2);
            codeInfo.MethodList[0].Name.Should().Be("Init");
            codeInfo.MethodList[1].Name.Should().Be("InitCopy");
        }

        [Fact]
        public void SimpleFileStatements()
        {
            var codeInfo = new MethodExtractor().Execute(new SourceCodeFile(@"..\..\..\..\assets\TestData\simpleFile.prg"));
            codeInfo.MethodList[0].StatementList.Should().HaveCount(11);
            codeInfo.MethodList[1].StatementList.Should().HaveCount(11);
        }
    }
}
