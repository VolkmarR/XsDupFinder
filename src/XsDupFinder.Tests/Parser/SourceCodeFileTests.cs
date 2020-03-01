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
    public class SourceCodeFileTests
    {
        [Fact]
        public void LoadFileExists()
        {
            var scf = new SourceCodeFile(@"..\..\..\..\assets\TestData\simpleFile.prg");
            scf.Should().NotBeNull();
            scf.SourceCode.Should().NotBeEmpty();
        }

        [Fact]
        public void LoadFileNotExists()
        {
            var scf = new SourceCodeFile(@"..\..\..\..\assets\TestData\FileNotExists.prg");
            scf.Should().NotBeNull();
            scf.SourceCode.Should().BeEmpty();
        }
    }
}
