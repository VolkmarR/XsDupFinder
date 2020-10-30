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

        [Fact]
        public void AllCodeBlocksFile()
        {
            var codeInfo = new MethodExtractor().Execute(new SourceCodeFile(@"..\..\..\..\assets\TestData\allCodeBlocks.prg"));
            codeInfo.MethodList.Should().HaveCount(10);

            codeInfo.MethodList.Select(q => q.Name).ToList().Should().BeEquivalentTo(
                "AsProperty[Get]", 
                "AsProperty[Set]", 
                "Operator", 
                "AsAccess", 
                "AsAssign", 
                "AsMethod", 
                "Constructor", 
                "Destructor", 
                "AsFunction", 
                "AsProcedure");

            codeInfo.MethodList.Select(q => q.Type).ToList().Should().BeEquivalentTo(
                MethodInfoType.PropertyGet, 
                MethodInfoType.PropertySet, 
                MethodInfoType.Operator, 
                MethodInfoType.Method, 
                MethodInfoType.Method, 
                MethodInfoType.Method, 
                MethodInfoType.Constructor, 
                MethodInfoType.Destructor, 
                MethodInfoType.FuncProc, 
                MethodInfoType.FuncProc);

            codeInfo.MethodList.Select(q => q.ClassName).ToList().Should().BeEquivalentTo(
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "DummyClass",
                "",
                "");

            foreach (var method in codeInfo.MethodList)
                method.StatementList.Should().HaveCount(3);
        }

        [Fact]
        public void SimpleFileClassName()
        {
            var codeInfo = new MethodExtractor().Execute(new SourceCodeFile(@"..\..\..\..\assets\TestData\simpleFile.prg"));
            codeInfo.MethodList.Should().HaveCount(2);
            foreach(var method in codeInfo.MethodList)
                method.ClassName.Should().Be("clsTestClass");
        }

    }
}
