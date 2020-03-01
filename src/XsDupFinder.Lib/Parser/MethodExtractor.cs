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
            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                foreach (var methodtypeContext in context.methodtype())
                {
                    Console.WriteLine(methodtypeContext.GetText());
                }

                Console.WriteLine(context.Sig.GetText());

                Console.WriteLine(context.statementBlock().GetText());
            }
        }

        public void Execute(string fileName)
        {
            var lexer = XSharpLexer.Create(File.ReadAllText(fileName), fileName);
            lexer.RemoveErrorListeners();

            var tokenStream = new CommonTokenStream(lexer, 0);
            var parser = new XSharpParser(tokenStream);
            parser.RemoveErrorListeners();

            var source = parser.source();
            var listener = new MethodListener();

            new ParseTreeWalker().Walk(listener, source);
        }

    }
}
