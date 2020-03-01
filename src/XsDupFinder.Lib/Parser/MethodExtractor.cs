using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Misc;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsDupFinder.Lib.Parser
{
    public class MethodExtractor
    {
        class MethodListener : XSharpBaseListener
        {
            public List<MethodInfo> MethodList = new List<MethodInfo>();

            string GetMethodName(XSharpParser.SignatureContext context)
                => context.Id.GetText();
            

            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                var method = new MethodInfo { Name = GetMethodName(context.Sig) };
                MethodList.Add(method);
            }
        }

        SourceCodeFile SourceCodeFile;

        List<MethodInfo> GetMethodInfos()
        {
            var lexer = XSharpLexer.Create(SourceCodeFile.SourceCode, SourceCodeFile.FileName);
            lexer.RemoveErrorListeners();

            var tokenStream = new CommonTokenStream(lexer, 0);
            var parser = new XSharpParser(tokenStream);
            parser.RemoveErrorListeners();

            var source = parser.source();
            var listener = new MethodListener();

            new ParseTreeWalker().Walk(listener, source);

            return listener.MethodList;
        }

        public CodeInfo Execute(SourceCodeFile sourceCodeFile)
        {
            SourceCodeFile = sourceCodeFile;
            return new CodeInfo { FileName = SourceCodeFile.FileName, HashCode = SourceCodeFile.HashCode, MethodList = GetMethodInfos() };
        }
    }
}
