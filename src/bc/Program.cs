using Biza.CodeAnalysis;
using Biza.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Biza
{
    class Program
    {
        static void Main(string[] args)
        {
            var showTree = true;
            var variables = new Dictionary<string, object>();

            while (true)
            {
                Write("> ");
                var line = ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = true;
                    WriteLine(showTree ? "Showing parse tree" : "Not showing parse tree");
                    continue;
                }
                else if (line == "cls")
                {
                    Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);

                var result = compilation.Evaluate(variables);

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!result.Diagnostics.Any())
                {
                    WriteLine(result.Value);
                }
                else
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        WriteLine();

                        ForegroundColor = ConsoleColor.DarkRed;
                        WriteLine(diagnostic);
                        ResetColor();

                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);

                        Write("    ");
                        Write(prefix);

                        ForegroundColor = ConsoleColor.DarkRed;
                        Write(error);
                        ResetColor();

                        Write(suffix);
                        WriteLine();
                    }

                    WriteLine();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Write(indent);
            Write(marker);
            Write(node.Kind);

            if (node is SyntaxToken t && t.Value is not null)
            {
                Write(" ");
                Write(t.Value);
            }

            WriteLine();

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}
