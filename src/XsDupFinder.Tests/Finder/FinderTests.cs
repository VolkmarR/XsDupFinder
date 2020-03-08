using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Parser;
using Xunit;

namespace XsDupFinder.Tests.Finder
{
    public class FinderTests
    {
        void AddFile(DuplicateFinder duplicateFinder, string fileName)
        {
            var sourceCodeFile = new SourceCodeFile(fileName);
            var codeInfo = new MethodExtractor().Execute(sourceCodeFile);
            duplicateFinder.AddSourceCodeFile(sourceCodeFile, codeInfo);
        }

        [Fact]
        public void NoDuplicats()
        {
            var fileName = @"..\..\..\..\assets\TestData\noDuplicates.prg";
            var duplicateFinder = new DuplicateFinder(new Configuration { SourceDirectory = Path.GetDirectoryName(fileName), MinLineForDuplicate = 10 }.FixOptionalValues());
            AddFile(duplicateFinder, fileName);

            duplicateFinder.Execute().Should().BeEmpty();
        }

        [Fact]
        public void OneFileWithDuplicate()
        {
            var fileName = @"..\..\..\..\assets\TestData\simpleFile.prg";
            var duplicateFinder = new DuplicateFinder(new Configuration { SourceDirectory = Path.GetDirectoryName(fileName), MinLineForDuplicate = 10 }.FixOptionalValues());
            AddFile(duplicateFinder, fileName);

            var result = duplicateFinder.Execute();
            result.Should().HaveCount(1);
            result[0].Locations.Should().HaveCount(2);
        }

        [Fact]
        public void TwoFilesWithDuplicate()
        {
            var fileName = @"..\..\..\..\assets\TestData\simpleFile.prg";
            var duplicateFinder = new DuplicateFinder(new Configuration { SourceDirectory = Path.GetDirectoryName(fileName), MinLineForDuplicate = 10 }.FixOptionalValues());
            AddFile(duplicateFinder, fileName);
            AddFile(duplicateFinder, @"..\..\..\..\assets\TestData\noDuplicates.prg");

            var result = duplicateFinder.Execute();
            result.Should().HaveCount(1);

            result[0].Locations.Should().HaveCount(3);
        }

        [Fact]
        public void DirectoryDuplicateFinder()
        {
            var configuration = new Configuration { SourceDirectory = Path.GetDirectoryName(@"..\..\..\..\assets\TestData\simpleFile.prg"), MinLineForDuplicate = 10 }.FixOptionalValues();
            var directoryDuplicateFinder = new DirectoryDuplicateFinder(configuration);

            var result = directoryDuplicateFinder.Execute();
            result.Should().HaveCount(1);

            result[0].Locations.Should().HaveCount(3);
        }
    }
}
