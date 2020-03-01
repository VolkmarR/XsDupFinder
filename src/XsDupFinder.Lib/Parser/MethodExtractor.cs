﻿using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
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

            readonly StringBuilder SBLine = new StringBuilder();
            int CurrentStart = -1;
            int CurrentStartLine = 0;

            private void AddElementToLine(IParseTree block)
            {
                SBLine.Append(block.GetText());
                SBLine.Append(' ');
            }

            private string GetCurrentLine()
            {
                var result = SBLine.ToString();
                SBLine.Length = 0;
                return result;
            }

            private void RenderStatements(IParseTree block, MethodInfo methodInfo)
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


            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                var methodInfo = new MethodInfo { Name = GetMethodName(context.Sig) };
                MethodList.Add(methodInfo);

                RenderStatements(context.statementBlock(), methodInfo);
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
