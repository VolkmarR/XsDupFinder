using LanguageService.CodeAnalysis.XSharp;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Misc;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XsDupFinder.Lib.Parser
{
    public class MethodExtractor
    {
        class MethodListener : XSharpBaseListener
        {
            public List<MethodInfo> MethodList = new List<MethodInfo>();

            string GetMethodName(XSharpParser.SignatureContext context)
                => context.Id.GetText();

            readonly StringBuilder SBLine = new StringBuilder();
            int CurrentStart = -1;
            int CurrentStartLine = 0;

            void AddElementToLine(IParseTree block)
            {
                SBLine.Append(block.GetText());
                SBLine.Append(' ');
            }

            string GetCurrentLine()
            {
                var result = SBLine.ToString();
                SBLine.Length = 0;
                return result;
            }

            void RenderStatements(IParseTree block, MethodInfo methodInfo)
            {
                if (block.ChildCount == 0)
                {
                    AddElementToLine(block);
                    return;
                }

                for (int i = 0; i < block.ChildCount; i++)
                {
                    var child = block.GetChild(i);

                    if (CurrentStart == -1)
                    {
                        if (child is TerminalNodeImpl && ((TerminalNodeImpl)child).symbol is XSharpToken token)
                        {
                            CurrentStart = token.Position;
                            CurrentStartLine = token.Line;
                        }
                        else if (child is XSharpParserRuleContext context)
                        {
                            CurrentStart = context.Position;
                            CurrentStartLine = context.Start?.Line ?? -1;
                        }
                        else
                            throw new Exception("Dump");
                    }

                    if (child is XSharpParser.EosContext eosChild)
                    {
                        methodInfo.StatementList.Add(new StatementInfo(GetCurrentLine(), CurrentStart, eosChild.Position, CurrentStartLine));
                        CurrentStart = -1;
                    }
                    else
                    {
                        RenderStatements(child, methodInfo);
                    }
                }
            }

            void AddMethodInfo(string name, MethodInfoType type, XSharpParser.StatementBlockContext statementBlockContext)
            {
                var methodInfo = new MethodInfo { Name = name, Type = type };
                MethodList.Add(methodInfo);

                RenderStatements(statementBlockContext, methodInfo);
            }

            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                if (context?.Sig == null)
                    return;

                AddMethodInfo(GetMethodName(context.Sig), MethodInfoType.Method, context.statementBlock());
            }

            public override void EnterConstructor([NotNull] XSharpParser.ConstructorContext context)
                => AddMethodInfo("Constructor", MethodInfoType.Constructor, context.statementBlock());

            public override void EnterDestructor([NotNull] XSharpParser.DestructorContext context)
                => AddMethodInfo("Destructor", MethodInfoType.Destructor, context.statementBlock());

            public override void EnterFuncproc([NotNull] XSharpParser.FuncprocContext context)
                => AddMethodInfo(GetMethodName(context.Sig), MethodInfoType.FuncProc, context.statementBlock());
        }

        SourceCodeFile SourceCodeFile;

        List<MethodInfo> GetMethodInfos()
        {
            var lexer = XSharpLexer.Create(SourceCodeFile.SourceCode, SourceCodeFile.FileName);
            lexer.RemoveErrorListeners();

            var tokenStream = new CommonTokenStream(lexer, 0);
            var parser = new XSharpParser(tokenStream);
            parser.Options = new XSharpParseOptions();
            parser.Options.SetXSharpSpecificOptions(XSharpSpecificCompilationOptions.Default);
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
