﻿using LanguageService.CodeAnalysis.XSharp;
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

            string GetClassName(XSharpParserRuleContext context)
            {
                var parent = context?.Parent;
                while (parent != null)
                {
                    if (parent is XSharpParser.Class_Context classContent)
                        return classContent.Id?.GetText() ?? "";

                    parent = parent.Parent;
                }

                return "";
            }

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

            void AddMethodInfo(string name, string className, MethodInfoType type, XSharpParser.StatementBlockContext statementBlockContext)
            {
                var methodInfo = new MethodInfo { Name = name, ClassName = className, Type = type };
                MethodList.Add(methodInfo);

                RenderStatements(statementBlockContext, methodInfo);
            }

            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                if (context?.Sig == null)
                    return;

                AddMethodInfo(GetMethodName(context.Sig), GetClassName(context), MethodInfoType.Method, context.statementBlock());
            }

            public override void EnterConstructor([NotNull] XSharpParser.ConstructorContext context)
                => AddMethodInfo("Constructor", GetClassName(context), MethodInfoType.Constructor, context.statementBlock());

            public override void EnterDestructor([NotNull] XSharpParser.DestructorContext context)
                => AddMethodInfo("Destructor", GetClassName(context), MethodInfoType.Destructor, context.statementBlock());

            public override void EnterFuncproc([NotNull] XSharpParser.FuncprocContext context)
                => AddMethodInfo(GetMethodName(context.Sig), "", MethodInfoType.FuncProc, context.statementBlock());

            public override void EnterOperator_([NotNull] XSharpParser.Operator_Context context) 
                => AddMethodInfo("Operator", GetClassName(context), MethodInfoType.Operator, context.statementBlock());

            public override void EnterPropertyAccessor([NotNull] XSharpParser.PropertyAccessorContext context)
            {
                if (!(context.parent is XSharpParser.PropertyContext idContext) || string.IsNullOrWhiteSpace(idContext.Id?.GetText())) 
                    return;

                MethodInfoType methodInfoType;
                if (string.Equals(context.Key.Text, "Get", StringComparison.OrdinalIgnoreCase))
                    methodInfoType = MethodInfoType.PropertyGet;
                else if (string.Equals(context.Key.Text, "Set", StringComparison.OrdinalIgnoreCase))
                    methodInfoType = MethodInfoType.PropertySet;
                else
                    throw new ArgumentException("Invalid PropertyAccessor Key");

                var name = $"{idContext.Id.GetText()}[{(methodInfoType == MethodInfoType.PropertyGet ? "Get" : "Set")}]";
                AddMethodInfo(name, GetClassName(context), methodInfoType, context.statementBlock());
            }
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
